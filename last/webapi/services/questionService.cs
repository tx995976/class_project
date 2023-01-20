using System.Diagnostics;

namespace webapi.service;

/*
<summary>
    #问题生成
    #问题分发
    #答案验证
        @分数公式 score = level * accept_num

<\summary>
*/
public class questionService
{
    public List<List<question>>? questions { get; set; }
    public List<List<string>>? answers { get; set; }

    public static start_info? start_token;

    public int each_level_cases { get; set; } = 15;
    public int each_cases { get; set; } = 10;
    public int level { get; set; } = 3; //nums = level * 2 + 1

    public int time_limit { get; set; } = 4000;

    public int level_case(int cases) => cases / each_level_cases + 1;

    Random random = new();
    ILogger<questionService> logger;
    dataService db;

    public questionService(ILogger<questionService> _logger,dataService _db) {
        logger = _logger;
        db = _db;
     }


    #region Observer Actions

    public event Action<judge_result>? answer_scores;
    public event Action<answers>? receive_answers;
    public event Action<fetch_info>? fetch_problems;

    #endregion

    #region question operations

    public List<question> get_question(int cases,long uuid){
        var info_server = db.subservers[uuid];
        fetch_problems?.Invoke(new fetch_info{
            hostname = info_server.hostname,
            uuid = uuid,
            num_case = cases
        });
        logger.LogInformation("Server {0} fetched case {1}",uuid,cases);
        return questions![cases];
    }

    #endregion

    #region question init

    async public Task init_questions() {
        var all_cases = level * each_level_cases;
        
        var arr_ques = new List<question>[all_cases];
        var arr_answer = new List<string>[all_cases];

        logger.LogInformation("start init question | total case: {0}",all_cases);
        var timer = new Stopwatch();
        timer.Start();
        // Init
        List<Task> tasks = new();
        for (int i = 1;i <= level;i++){
            for (int j = 1;j <= each_level_cases;j++) {
                var task = init_case(i,j,arr_ques,arr_answer);
                tasks.Add(task);
            }
        }
        await Task.WhenAll(tasks);
        questions = arr_ques.ToList();
        answers = arr_answer.ToList();
        timer.Stop();
        logger.LogInformation("init complete in {0} ms",timer.ElapsedMilliseconds);

        start_token = new start_info{
            all_cases = level * each_level_cases,
            time_limit = time_limit
        };

        //TODO: save to file
        
    }

    async Task init_case(int level,int case_no,List<question>[] arr_ques,List<string>[] arr_answers) {
        var init_num = level * 2 + 1;
        var case_pos = (level - 1) * each_level_cases + case_no - 1;

        List<question> cases = new();
        List<string> operation = new();

        for (int i = 0;i < each_cases;i++) {
            List<int> nums = new();
            string opera = "";
            lock (random) {
                for (int j = 0;j < init_num;j++)
                    nums.Add(random.Next(1, 10));
                for (int j = 0;j < init_num - 1;j++)
                    opera += random.Next(0, 4).ToString();
            }
            var result = check_result(nums, opera);

            cases.Add(new question
            {
                nums = nums,
                result = result
            });
            operation.Add(opera);
        }
        //Console.WriteLine($"in case {case_pos}");
        arr_ques[case_pos] = cases;
        arr_answers[case_pos] = operation;
        await Task.CompletedTask;
    }

    #endregion

    #region question verify

    public static int check_result(List<int> nums, string opera) {
        var first = nums.First();
        for (int i = 1;i < nums.Count;i++) {
            first = opera[i - 1] switch
            {
                '0' => first + nums[i],
                '1' => first - nums[i],
                '2' => first * nums[i],
                '3' => first / nums[i],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        return first;
    }

    async public Task<int> answer_verify(answers ans) {
        var ans_str = ans.answer!;
        var question = questions![ans.problem_case];
        var init_ans = answers![ans.problem_case];
        var q_level = level_case(ans.problem_case);

        logger.LogInformation("Server {0} commit case {1}",ans.uuid,ans.problem_case);
        receive_answers?.Invoke(ans); //
        //HACK: add delay
        await Task.Delay(1000);

        int score = 0;

        for (int i = 0;i < ans_str.Count;i++) {
            if (ans_str[i] == init_ans[i])
                score += q_level;
            else if (ans_str[i] is "" or "null" || ans_str[i].Length != init_ans[i].Length)
                continue;
            else {
                if (check_result(question[i]!.nums!, ans_str[i]) == question[i].result)
                    score += q_level;
                //ADDON: 保存验证通过答案
            }
        }

        answer_scores?.Invoke(new judge_result
        {
            uuid = ans.uuid,
            problem_case = ans.problem_case,
            score = score,
        });

        return score;
    }

    #endregion

}