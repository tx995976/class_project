using System.Collections.Generic;
using System.Text;

namespace shared.Model;

#region questions_model

public record class question
{
    public List<int>? nums { get; set; }
    public int result { get; set; }
}


public record class answers
{
    public int problem_case { get; set; }
    public long uuid { get; set; }
    public List<string>? answer { get; set; }
    public DateTime init_time { get; init; } = DateTime.Now;
}

public record class judge_result
{
    public long uuid { get; set; }
    public int problem_case { get; set; }
    public int score { get; set; }
    public DateTime judge_time { get; init; } = DateTime.Now;
}

#endregion

#region info model

public record class reg_info
{
    public bool success { get; set; }
    public long uuid { get; set; }
}

public record class start_info
{
    public bool Iscancelltoken { get; set; } = false;
    public int time_limit { get; set; }
    public int all_cases { get; set; }
}

public record class subserver_info
{
    public string? hostname { get; set; }
    public string? name { get; set; }
    public int group { get; set; }
    public long uuid { get; set; }
    public bool Isactive { get; set; } = false;
}

public record class fetch_info
{
    public string? hostname { get; set; }
    public long uuid { get; set; }
    public int num_case { get; set; }
    public DateTime fetchtime { get; init; } = DateTime.Now;
}

#endregion


static public class Extend_model_method
{
    public static string get_result_str(this judge_result data) {
        return $"result|{data.problem_case}|{data.score}";
    }

    public static string get_commit_str(this answers data) {
        return $"status|{data.problem_case}|commit";
    }

    public static string get_fetch_str(this fetch_info data) {
        return $"status|{data.num_case}|fetch";
    }


}