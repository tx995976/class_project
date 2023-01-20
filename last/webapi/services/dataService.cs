namespace webapi.service;

public class dataService
{
    public Dictionary<long,subserver_info> subservers = new();  

    public Dictionary<long,List<judge_result>> oj_results = new();
    public Dictionary<long,List<answers>> commit_list = new();
    public Dictionary<long,List<fetch_info>> fetch_list = new();

    public int[] group_dashboard = new int[ObserverService.groups];

    ILogger<dataService> logger;

    public event Action<string>? score_up_result;

    public dataService(ILogger<dataService> _logger){
        logger = _logger;
    }
 
    public void start(){
        var ques_service = App.get_service<questionService>()!;
        ques_service.answer_scores += judge_result_collection;
        ques_service.receive_answers += answers_collection;
        ques_service.fetch_problems += fetch_collection;

        logger.LogInformation("data service started");
    }

    #region data_search
    
    public subserver_info? get_subserverByhost(string hostname) =>
        subservers.Values.Where(x => x.hostname == hostname).SingleOrDefault();
    
    #endregion

    #region data_collection

    void judge_result_collection(judge_result result){
        score_up(result);
        oj_results[result.uuid].Add(result);
    }   

    void answers_collection(answers data){
        commit_list[data.uuid].Add(data);
    }

    void fetch_collection(fetch_info data){
        fetch_list[data.uuid].Add(data);
    }

    #endregion

    #region score_sort

    void score_up(judge_result jd_res){
        int pre_max = 0;
        var same_record = oj_results[jd_res.uuid]
            .Where(x => x.problem_case == jd_res.problem_case);
        if(same_record.Any())
            pre_max = same_record.Select(x => x.score).Max();

        var group = subservers[jd_res.uuid].group;
        group_dashboard[group] += pre_max >= jd_res.score ? 0 : jd_res.score - pre_max;

        //init str
        var result = "score|-";
        
        foreach(var pair in group_dashboard)
            result += pair.ToString() + '-';
        //result.TrimEnd('-');
        score_up_result?.Invoke(result);
    }

    #endregion

    #region data_analysis
    // ADDON: 节点做题情况综合评分

    #endregion
}