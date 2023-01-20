using shared.Model;
using subserver.calculate;
using System.Diagnostics;

namespace subserver.services;
/*
    #reg subserver
    #wait master port
    #start calculate 
        ! excced time limit
    #get case master
    #push result --> master
*/

public class CalculateService
{
    public Inode_calculator? alo;
    
    public int cal_thread { get; set; } = 5;
    public int time_limit { get; set; } = 6000;
    public int all_cases { get; set; }

    public event Action<List<String>>? answer_produce;

    private ILogger<CalculateService> _logger;
    private CancellationTokenSource _token = new();

    public CalculateService(ILogger<CalculateService> logger){
        _logger = logger;

    }

    public void defuse_cal() =>
        _token.Cancel();

    async public Task start_calculate() {
        Console.WriteLine("\u001B[32minfo: \u001B[0mstart calculate");
        _token = new();

        int case_now = 0;
        while (case_now < all_cases) {
            Console.WriteLine($"\u001B[34mcalculate: \u001B[0min case {case_now}");

            //data init
            var problems = await get_problem(case_now);
            var arr_result = new string[problems.Count];
            for(int i = 0; i < problems.Count; i++)
                arr_result[i] = "";
            //exec
            try{
                await execute_cal(problems,arr_result,_token.Token);
            }
            catch(OperationCanceledException){
                _logger.LogWarning("operation has been cancelled by master");
                break;
            }

            var result = arr_result.ToList();
            var score = await commit_results(case_now,App.uuid,result);
            answer_produce?.Invoke(result);
            Console.WriteLine($"\u001B[32mcase {case_now} result:\u001B[0m score is {score}");

            case_now++;
        }
        Console.WriteLine($"\u001B[32minfo: \u001B[0m calculation complete");

    }

    async Task<List<question>> get_problem(int problem_case) {
        var request = new HttpClient{
            BaseAddress = new Uri(App.master_url)
        };
        var vaild_case = problem_case < all_cases ? problem_case : all_cases - 1;
        var data = await request.GetFromJsonAsync<List<question>>($"api/question?problem_case={vaild_case}&uuid={App.uuid}");
        return data!;
    }

    async Task execute_cal(List<question> problems,string[] result,CancellationToken token) {
        var test_case = 0;
        var all_cases = problems.Count;

        var tasks = new List<Task>();
        for (int i = 0;i < cal_thread;i++) {
            tasks.Add(Task.Run(async() =>
            {
                while (true) {
                    token.ThrowIfCancellationRequested();
                    question input;
                    int cases;

                    lock (problems) {
                        if (test_case == all_cases)
                            return;
                        cases = test_case;
                        input = problems[test_case++];
                    }

                    var res = await alo!.cal_case(input);
                    string res_str = "";
                    res.ForEach(num => res_str += num.ToString());
                    result[cases] = res_str;
                }
            }));
        }

        try{
            await Task.WhenAll(tasks.ToArray());
        }
        catch(OperationCanceledException){
            token.ThrowIfCancellationRequested();
        }
    }


    async Task<int> commit_results(int problem_case, long uuid, List<string> answer) {
        var post_client = new HttpClient{
            BaseAddress = new Uri(App.master_url)
        };
        
        var data = new answers
        {
            problem_case = problem_case,
            uuid = uuid,
            answer = answer,
        };
        var request = await post_client.PostAsJsonAsync($"api/verify", data);
        //ADDON: kafka client
        var res = await request.Content.ReadAsStringAsync();
        _logger.LogDebug("result_str {0}",res);
        return int.Parse(res);
    }
}
