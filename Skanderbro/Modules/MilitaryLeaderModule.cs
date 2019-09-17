using System.Threading.Tasks;
using Discord.Commands;
using Skanderbro.Models;
using Skanderbro.Models.Enums;
using Skanderbro.Services;
using Skanderbro.Strategies.LeaderGeneration;
using Skanderbro.Validators;

namespace Skanderbro.Modules
{
    public sealed class MilitaryLeaderModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILeaderPipService leaderPipService;

        public MilitaryLeaderModule(ILeaderPipService leaderPipService)
        {
            this.leaderPipService = leaderPipService;
        }

        // !general [tradition]
        [Command("general")]
        [Summary("Approximate average pips for a general, given military tradition.")]
        public async Task ApproximateGeneralPipsAsync(
            [Summary("Army tradition (0-100)")] double tradition,
            [Summary("Guaranteed pips (0-6 in each category)")] LeaderPipModifiers pipModifiers = null)
        {
            if (!LeaderValidator.IsTraditionValid(tradition, out string traditionError))
            {
                await ReplyAsync($"Argument error: {traditionError}.");
                return;
            }

            var leaderPipResult = await Task.Run(() => leaderPipService.CalculateAverageLeaderPips(
                tradition,
                LeaderType.General,
                new LeaderPipApproximationStrategy(),
                pipModifiers));
            await ReplyAsync($"Average general pips: Fire {leaderPipResult.Fire}, Shock {leaderPipResult.Shock}, Maneuver {leaderPipResult.Maneuver}, Siege {leaderPipResult.Siege}");
        }

        // !simulate-general [tradition]
        [Command("simulate-general")]
        [Summary("Uses simulation to calculate average pips for a general, given military tradition.")]
        public async Task SimulateGeneralPipsAsync(
            [Summary("Army tradition (0-100)")] double tradition,
            [Summary("Guaranteed pips (0-6 in each category)")] LeaderPipModifiers pipModifiers = null)
        {
            if (!LeaderValidator.IsTraditionValid(tradition, out string traditionError))
            {
                await ReplyAsync($"Argument error: {traditionError}.");
                return;
            }

            var leaderPipResult = await Task.Run(() => leaderPipService.CalculateAverageLeaderPips(
                tradition,
                LeaderType.General,
                new LeaderPipSimulationStrategy(),
                pipModifiers));
            await ReplyAsync($"Average general pips: Fire {leaderPipResult.Fire}, Shock {leaderPipResult.Shock}, Maneuver {leaderPipResult.Maneuver}, Siege {leaderPipResult.Siege}");
        }

        // !ruler-general [tradition] [militarySkill]
        [Command("ruler-general")]
        [Summary("Calculate average pips for a ruler/heir general, given military tradition and military skill.")]
        public async Task ApproximateRulerGeneralPipsAsync(
            [Summary("Army tradition (0-100)")] double tradition,
            [Summary("Ruler military skill (0-6)")] int militarySkill,
            [Summary("Guaranteed pips (0-6 in each category)")] LeaderPipModifiers pipModifiers = null)
        {
            if (!LeaderValidator.IsTraditionValid(tradition, out string traditionError))
            {
                await ReplyAsync($"Argument error: {traditionError}.");
                return;
            }
            if (!LeaderValidator.IsRulerMilitarySkillValid(militarySkill, out string militarySkillError))
            {
                await ReplyAsync($"Argument error: {militarySkillError}.");
                return;
            }

            var leaderPipResult = await Task.Run(() => leaderPipService.CalculateAverageRulerLeaderPips(
                tradition,
                militarySkill,
                new LeaderPipApproximationStrategy(),
                pipModifiers));
            await ReplyAsync($"Average ruler general pips: Fire {leaderPipResult.Fire}, Shock {leaderPipResult.Shock}, Maneuver {leaderPipResult.Maneuver}, Siege {leaderPipResult.Siege}");
        }

        // !simulate-ruler-general [tradition]
        [Command("simulate-ruler-general")]
        [Summary("Uses simulation to calculate average pips for a ruler/heir general, given military tradition and military skill.")]
        public async Task SimulateRulerGeneralPipsAsync(
            [Summary("Army tradition (0-100)")] double tradition,
            [Summary("Ruler military skill (0-6)")] int militarySkill,
            [Summary("Guaranteed pips (0-6 in each category)")] LeaderPipModifiers pipModifiers = null)
        {
            if (!LeaderValidator.IsTraditionValid(tradition, out string traditionError))
            {
                await ReplyAsync($"Argument error: {traditionError}.");
                return;
            }
            if (!LeaderValidator.IsRulerMilitarySkillValid(militarySkill, out string militarySkillError))
            {
                await ReplyAsync($"Argument error: {militarySkillError}.");
                return;
            }

            var leaderPipResult = await Task.Run(() => leaderPipService.CalculateAverageLeaderPips(
                tradition,
                LeaderType.General,
                new LeaderPipSimulationStrategy(),
                pipModifiers));
            await ReplyAsync($"Average general pips: Fire {leaderPipResult.Fire}, Shock {leaderPipResult.Shock}, Maneuver {leaderPipResult.Maneuver}, Siege {leaderPipResult.Siege}");
        }
    }
}
