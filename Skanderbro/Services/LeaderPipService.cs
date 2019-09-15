using System;
using Skanderbro.Models;
using Skanderbro.Models.Enums;

namespace Skanderbro.Services
{
    public sealed class LeaderPipService : ILeaderPipService
    {
        public LeaderPipResult CalculateAverageLeaderPips(double tradition, LeaderType leaderType)
        {
            if (tradition < 0 || tradition > 100)
            {
                throw new ArgumentOutOfRangeException($"Tradition is out of range: {tradition}");
            }
            double effectiveTradition = CalculateEffectiveTradition(tradition, leaderType);
            double averagePips = CalculateAveragePips(effectiveTradition);
            return DistributePips(averagePips);
        }

        public LeaderPipResult CalculateAverageRulerLeaderPips(double tradition, int militarySkill)
        {
            if (tradition < 0 || tradition > 100)
            {
                throw new ArgumentOutOfRangeException($"Tradition is out of range: {tradition}");
            }
            if (militarySkill < 0 || militarySkill > 6)
            {
                throw new ArgumentOutOfRangeException($"Military skill is out of range: {militarySkill}");
            }
            double effectiveTradition = CalculateRulerEffectiveTradition(tradition, militarySkill);
            double averagePips = CalculateAveragePips(effectiveTradition);
            return DistributePips(averagePips);
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

        private LeaderPipResult DistributePips(double pips)
        {
            double remainingPips = pips;
            var result = new LeaderPipResult();
            while (remainingPips > 10)
            {
                remainingPips -= 4;
                result.Fire++;
                result.Shock++;
                result.Maneuver++;
                result.Siege++;
            }
            result.Fire += Math.Round(0.3 * remainingPips, 2);
            result.Shock += Math.Round(0.3 * remainingPips, 2);
            result.Maneuver += Math.Round(0.3 * remainingPips, 2);
            result.Siege += Math.Round(0.1 * remainingPips, 2);

            return result;
        }
    }
}
