using Confluent.Kafka;
using System.Collections.Generic;

using shared.Model;

namespace subserver.calculate;

public interface Inode_calculator
{
    public Task<List<int>> cal_case(question data);
}
