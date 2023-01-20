using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("/api")]
public class ContestController : ControllerBase
{

    [HttpGet("question")]
    public List<question> get_problem(int problem_case,long uuid){
        var service = App.get_service<service.questionService>()!;
        return service.get_question(problem_case, uuid);
    }

    /// 验证答案
    [HttpPost("verify")]
    async public Task<int> verify_answer(answers data){
        var service = App.get_service<service.questionService>()!;
        return await service.answer_verify(data);
    }

    [HttpGet("server/initcase")]
    async public Task<int> init_questions(int level,int each_level_cases,int each_cases){
        var service = App.get_service<service.questionService>()!;
        service.level = level;
        service.each_level_cases = each_level_cases;
        service.each_cases = each_cases;
        await service.init_questions();

        return level*each_level_cases;
    }

    [HttpGet("server/contest")]
    public int controll_contest(bool status){
        if(status is true)
            App.get_service<service.ObserverService>()!.start_contest();
        else
            App.get_service<service.ObserverService>()!.stop_contest();
        
        return 200;
    }

}