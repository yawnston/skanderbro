using System.Threading.Tasks;
using Skanderbro.Models;

namespace Skanderbro.Strategies.LeaderGeneration
{
    public interface ILeaderPipDistributionStrategy
    {
        Task<LeaderPipResult> DistributePipsAsync(double averageBasePips, LeaderPipModifiers leaderPipModifiers = null);
    }
}
