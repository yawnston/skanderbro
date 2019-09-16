using System;
using Skanderbro.Constants;
using Skanderbro.Extensions;
using Skanderbro.Models;

namespace Skanderbro.Strategies.LeaderGeneration
{
    public sealed class LeaderPipSimulationStrategy : ILeaderPipDistributionStrategy
    {
        private const int SimulationIterations = 1000;
        private const int RoundingDigits = 2;

        private readonly Random random = new Random(DateTime.UtcNow.Millisecond);

        public LeaderPipResult DistributePips(double averageBasePips, LeaderPipModifiers leaderPipModifiers = null)
        {
            var result = new LeaderPipResult();
            for (int i = 0; i < SimulationIterations; i++)
            {
                LeaderPipResult iterationResult = DistributePipsSimulation(averageBasePips, leaderPipModifiers);

                result.Fire += iterationResult.Fire;
                result.Shock += iterationResult.Shock;
                result.Maneuver += iterationResult.Maneuver;
                result.Siege += iterationResult.Siege;
            }

            result.Fire = Math.Round(result.Fire / SimulationIterations, RoundingDigits);
            result.Shock = Math.Round(result.Shock / SimulationIterations, RoundingDigits);
            result.Maneuver = Math.Round(result.Maneuver / SimulationIterations, RoundingDigits);
            result.Siege = Math.Round(result.Siege / SimulationIterations, RoundingDigits);

            return result;
        }

        private LeaderPipResult DistributePipsSimulation(double averageBasePips, LeaderPipModifiers leaderPipModifiers)
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

            while (remainingPips > 0)
            {
                int which = random.Next(0, 10);
                double amount = remainingPips >= 1 ? 1 : remainingPips;

                if (which == 0 && result.Siege < LeaderConstants.MaxPipsInCategory)
                {
                    result.Siege += amount;
                    remainingPips -= amount;
                }
                else if (which < 4 && result.Shock < LeaderConstants.MaxPipsInCategory)
                {
                    result.Shock += amount;
                    remainingPips -= amount;
                }
                else if (which < 7 && result.Fire < LeaderConstants.MaxPipsInCategory)
                {
                    result.Fire += amount;
                    remainingPips -= amount;
                }
                else if (result.Maneuver < LeaderConstants.MaxPipsInCategory)
                {
                    result.Maneuver += amount;
                    remainingPips -= amount;
                }
                // else go back to generating new which
            }

            return result;
        }
    }
}
