using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skanderbro.Constants;
using Skanderbro.Extensions;
using Skanderbro.Models;

namespace Skanderbro.Strategies.LeaderGeneration
{
    public sealed class LeaderPipSimulationStrategy : ILeaderPipDistributionStrategy
    {
        private const int SimulationIterations = 1000;
        private const int DegreeOfParallelism = 4;
        private const int IterationsPerSlave = SimulationIterations / DegreeOfParallelism;
        private const int RoundingDigits = 2;

        public async Task<LeaderPipResult> DistributePipsAsync(double averageBasePips, LeaderPipModifiers leaderPipModifiers = null)
        {
            var slaveTasks = new List<Task<LeaderPipResult>>();
            for (int i = 0; i < DegreeOfParallelism; i++)
            {
                slaveTasks.Add(Task.Run(() => RunSimulation(IterationsPerSlave, averageBasePips, leaderPipModifiers)));
            }

            await Task.WhenAll(slaveTasks);

            var result = new LeaderPipResult();

            foreach (var slaveResult in slaveTasks.Select(s => s.Result))
            {
                result.Fire += slaveResult.Fire;
                result.Shock += slaveResult.Shock;
                result.Maneuver += slaveResult.Maneuver;
                result.Siege += slaveResult.Siege;
            }

            result.Fire = Math.Round(result.Fire / DegreeOfParallelism, RoundingDigits);
            result.Shock = Math.Round(result.Shock / DegreeOfParallelism, RoundingDigits);
            result.Maneuver = Math.Round(result.Maneuver / DegreeOfParallelism, RoundingDigits);
            result.Siege = Math.Round(result.Siege / DegreeOfParallelism, RoundingDigits);

            return result;
        }

        private static LeaderPipResult RunSimulation(int iterations, double averageBasePips, LeaderPipModifiers leaderPipModifiers = null)
        {
            var random = new Random(DateTime.UtcNow.Millisecond);
            var result = new LeaderPipResult();
            for (int i = 0; i < iterations; i++)
            {
                LeaderPipResult iterationResult = DistributePipsSimulation(averageBasePips, leaderPipModifiers, random);

                result.Fire += iterationResult.Fire;
                result.Shock += iterationResult.Shock;
                result.Maneuver += iterationResult.Maneuver;
                result.Siege += iterationResult.Siege;
            }

            result.Fire = Math.Round(result.Fire / iterations, RoundingDigits);
            result.Shock = Math.Round(result.Shock / iterations, RoundingDigits);
            result.Maneuver = Math.Round(result.Maneuver / iterations, RoundingDigits);
            result.Siege = Math.Round(result.Siege / iterations, RoundingDigits);

            return result;
        }

        private static LeaderPipResult DistributePipsSimulation(double averageBasePips, LeaderPipModifiers leaderPipModifiers, Random random)
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
