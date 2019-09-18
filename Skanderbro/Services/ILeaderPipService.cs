using Skanderbro.Models;
using Skanderbro.Models.Enums;
using Skanderbro.Strategies.LeaderGeneration;

namespace Skanderbro.Services
{
    public interface ILeaderPipService
    {
        LeaderPipResult CalculateAverageLeaderPips(
            double tradition,
            LeaderType leaderType,
            ILeaderPipDistributionStrategy leaderPipDistributionStrategy,
            LeaderPipModifiers leaderPipModifiers = null);

        LeaderPipResult CalculateAverageRulerLeaderPips(
            double tradition,
            int militarySkill,
            ILeaderPipDistributionStrategy leaderPipDistributionStrategy,
            LeaderPipModifiers leaderPipModifiers = null);
    }
}
