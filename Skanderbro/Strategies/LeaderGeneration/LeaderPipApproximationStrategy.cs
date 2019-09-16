using System;
using Skanderbro.Constants;
using Skanderbro.Extensions;
using Skanderbro.Models;

namespace Skanderbro.Strategies.LeaderGeneration
{
    public sealed class LeaderPipApproximationStrategy : ILeaderPipDistributionStrategy
    {
        public LeaderPipResult DistributePips(double averageBasePips, LeaderPipModifiers leaderPipModifiers = null)
        {
            double remainingPips = averageBasePips;
            var result = new LeaderPipResult();
            while (remainingPips > 10)
            {
                remainingPips -= 4;
                result.Fire++;
                result.Shock++;
                result.Maneuver++;
                result.Siege++;
            }

            result.AddGuaranteedPips(leaderPipModifiers ?? new LeaderPipModifiers());

            // The approximation here is that these percentages don't reflect pip overflow from maxed out categories
            result.Fire += Math.Round(0.3 * remainingPips, 2);
            result.Shock += Math.Round(0.3 * remainingPips, 2);
            result.Maneuver += Math.Round(0.3 * remainingPips, 2);
            result.Siege += Math.Round(0.1 * remainingPips, 2);

            result.Fire = result.Fire > LeaderConstants.MaxPipsInCategory ? LeaderConstants.MaxPipsInCategory : result.Fire;
            result.Shock = result.Shock > LeaderConstants.MaxPipsInCategory ? LeaderConstants.MaxPipsInCategory : result.Shock;
            result.Maneuver = result.Maneuver > LeaderConstants.MaxPipsInCategory ? LeaderConstants.MaxPipsInCategory : result.Maneuver;
            result.Siege = result.Siege > LeaderConstants.MaxPipsInCategory ? LeaderConstants.MaxPipsInCategory : result.Siege;

            return result;
        }
    }
}
