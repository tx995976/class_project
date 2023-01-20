using shared.Model;
using System.Linq;

namespace subserver.calculate;


// random answer
public class method1 : Inode_calculator
{
    static Random random = new();

    async public Task<List<int>> cal_case(question data)
    {
        var num = data.nums!.Count;
        await Task.Delay(2000);
        return Enumerable.Range(2, num)
                .Select(x => random.Next(0,4))
                .ToList();
    }


}