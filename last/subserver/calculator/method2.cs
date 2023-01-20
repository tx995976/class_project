using shared.Model;
using System.Linq;

namespace subserver.calculate;

public class method2 : Inode_calculator
{
    async public Task<List<int>> cal_case(question data)
    {
        var nums = data.nums!;
        var operations = nums.Count-1;
        var tar = data.result;

        //bfs
        //issue: 耗费大量内存,慎用
        var queue = new Queue<List<int>>();
        queue.Enqueue(new());
        while(queue.Count != 0){
            var tup = queue.Dequeue();
            if(tup.Count == operations){
                var first = nums.First();
                for (int i = 1;i < nums.Count;i++) {
                    first = tup[i - 1] switch
                    {
                    0 => first + nums[i],
                    1 => first - nums[i],
                    2 => first * nums[i],
                    3 => first / nums[i],
                    _ => throw new ArgumentOutOfRangeException()
                    };
                }
                if(first == tar)
                    return tup;
            }
            else{
                for(int i = 0; i < 4;i++){
                    List<int> ntup = new(tup);
                    ntup.Add(i);
                    queue.Enqueue(ntup);
                }
            }
        }
        await Task.CompletedTask;
        throw new NotSupportedException();
    }
}