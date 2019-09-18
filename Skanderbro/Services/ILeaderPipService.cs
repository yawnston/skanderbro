using System.Threading.Tasks;
using Skanderbro.Models;
using Skanderbro.Models.Enums;
using Skanderbro.Strategies.LeaderGeneration;

namespace Skanderbro.Services
{
    public interface ILeaderPipService
    {
        Task<LeaderPipResult> CalculateAverageLeaderPipsAsync(
            double tradition,
            LeaderType leaderType,
            ILeaderPipDistributionStrategy leaderPipDistributionStrategy,
            LeaderPipModifiers leaderPipModifiers = null);

        Task<LeaderPipResult> CalculateAverageRulerLeaderPipsAsync(
            double tradition,
            int militarySkill,
            ILeaderPipDistributionStrategy leaderPipDistributionStrategy,
            LeaderPipModifiers leaderPipModifiers = null);
    }
}
