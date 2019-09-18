using System;
using System.Threading.Tasks;
using Skanderbro.Models;
using Skanderbro.Models.Enums;
using Skanderbro.Strategies.LeaderGeneration;

namespace Skanderbro.Services
{
    public sealed class LeaderPipService : ILeaderPipService
    {
        public Task<LeaderPipResult> CalculateAverageLeaderPipsAsync(
            double tradition,
            LeaderType leaderType,
            ILeaderPipDistributionStrategy leaderPipDistributionStrategy,
            LeaderPipModifiers leaderPipModifiers = null)
        {
            double effectiveTradition = CalculateEffectiveTradition(tradition, leaderType);
            return CalculateAndDistributePips(leaderPipDistributionStrategy, leaderPipModifiers, effectiveTradition);
        }

        public Task<LeaderPipResult> CalculateAverageRulerLeaderPipsAsync(
            double tradition,
            int militarySkill,
            ILeaderPipDistributionStrategy leaderPipDistributionStrategy,
            LeaderPipModifiers leaderPipModifiers = null)
        {
            double effectiveTradition = CalculateRulerEffectiveTradition(tradition, militarySkill);
            return CalculateAndDistributePips(leaderPipDistributionStrategy, leaderPipModifiers, effectiveTradition);
        }

        private Task<LeaderPipResult> CalculateAndDistributePips(ILeaderPipDistributionStrategy leaderPipDistributionStrategy, LeaderPipModifiers leaderPipModifiers, double effectiveTradition)
        {
            double averagePips = CalculateAveragePips(effectiveTradition);
            return leaderPipDistributionStrategy.DistributePipsAsync(averagePips, leaderPipModifiers);
        }

        private double CalculateAveragePips(double effectiveTradition)
        {
            return 3.5
                + Math.Floor(effectiveTradition / 20)
                + CalculateBonusPipWithThreshold(effectiveTradition, 0)
                + CalculateBonusPipWithThreshold(effectiveTradition, 20)
                + CalculateBonusPipWithThreshold(effectiveTradition, 40)
                + CalculateBonusPipWithThreshold(effectiveTradition, 60)
                + CalculateBonusPipWithThreshold(effectiveTradition, 80)
                + 0.5;
        }

        private double CalculateBonusPipWithThreshold(double effectiveTradition, int threshold)
        {
            if (effectiveTradition > threshold)
            {
                return (effectiveTradition - threshold) / 100;
            }
            return 0;
        }

        private double CalculateEffectiveTradition(double tradition, LeaderType leaderType)
        {
            switch (leaderType)
            {
                case LeaderType.General:
                case LeaderType.Admiral:
                    return tradition;
                case LeaderType.Conquistador:
                case LeaderType.Explorer:
                    return 0.8 * tradition;
                default:
                    throw new ArgumentException($"Unknown leader type: {leaderType.ToString()}");
            }
        }

        private double CalculateRulerEffectiveTradition(double tradition, int militarySkill)
        {
            return (0.5 * tradition) + (7 * militarySkill);
        }
    }
}
