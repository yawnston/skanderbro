using Skanderbro.Models;

namespace Skanderbro.Strategies.LeaderGeneration
{
    public interface ILeaderPipDistributionStrategy
    {
        LeaderPipResult DistributePips(double effectiveTradition);
    }
}
