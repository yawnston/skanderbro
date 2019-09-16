using System;
using Skanderbro.Constants;
using Skanderbro.Models;

namespace Skanderbro.Extensions
{
    public static class LeaderPipResultExtensions
    {
        public static void AddGuaranteedPips(this LeaderPipResult leaderPipResult, LeaderPipModifiers leaderPipModifiers)
        {
            AddGuaranteedPipsSingleCategory(() => leaderPipResult.Fire, x => leaderPipResult.Fire = x, leaderPipModifiers.BonusFire);
            AddGuaranteedPipsSingleCategory(() => leaderPipResult.Shock, x => leaderPipResult.Shock = x, leaderPipModifiers.BonusShock);
            AddGuaranteedPipsSingleCategory(() => leaderPipResult.Maneuver, x => leaderPipResult.Maneuver = x, leaderPipModifiers.BonusManeuver);
            AddGuaranteedPipsSingleCategory(() => leaderPipResult.Siege, x => leaderPipResult.Siege = x, leaderPipModifiers.BonusSiege);
        }

        private static void AddGuaranteedPipsSingleCategory(Func<double> getter, Action<double> setter, int guaranteedPips)
        {
            double currentPips = getter();
            if (currentPips + guaranteedPips > LeaderConstants.MaxPipsInCategory)
            {
                setter(LeaderConstants.MaxPipsInCategory);
            }
            else
            {
                setter(currentPips + guaranteedPips);
            }
        }
    }
}
