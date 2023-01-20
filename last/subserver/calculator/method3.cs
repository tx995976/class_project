using shared.Model;
using System.Linq;

namespace subserver.calculate;

public class method3 : Inode_calculator
{
    async public Task<List<int>> cal_case(question data)
    {
        var nums = data.nums!;
        var operations = nums.Count-1;
        var tar = data.result;
        //dfs
        List<int> ans = new() , st = new();

        bool dfs(List<int> st){
            if(st.Count == operations){
                    var first = nums.First();
                    for (int i = 1;i < nums.Count;i++) {
                        first = st[i - 1] switch
                        {
                            0 => first + nums[i],
                            1 => first - nums[i],
                            2 => first * nums[i],
                            3 => first / nums[i],
                            _ => throw new ArgumentOutOfRangeException()
                        };
                    }
                    if(first == tar){
                        ans = st;
                        return true;
                    }
                    return false;
                }
                else{
                    for(int i = 0; i < 4;i++){
                        st.Add(i);
                        if(dfs(st))
                            return true;
                        st.RemoveAt(st.Count-1);
                    }
                }
                return false;
        }
        await Task.CompletedTask;
        if(!dfs(st))
            throw new NotSupportedException();
        return ans;
    }



}