using System.Threading.Tasks;
using Discord.Commands;
using Skanderbro.Models.Enums;
using Skanderbro.Services;

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
        [Summary("Calculate average pips for a general, given military tradition.")]
        public async Task CalculateGeneralPipsAsync([Summary("Army tradition (0-100)")] double tradition)
        {
            var leaderPipResult = await Task.Run(() => leaderPipService.CalculateAverageLeaderPips(tradition, LeaderType.General));
            await ReplyAsync($"Average general pips: Fire {leaderPipResult.Fire}, Shock {leaderPipResult.Shock}, Maneuver {leaderPipResult.Maneuver}, Siege {leaderPipResult.Siege}");
        }

        // !ruler-general [tradition] [militarySkill]
        [Command("ruler-general")]
        [Summary("Calculate average pips for a ruler/heir general, given military tradition and military skill.")]
        public async Task CalculateRulerGeneralPipsAsync(double tradition, int militarySkill)
        {
            var leaderPipResult = await Task.Run(() => leaderPipService.CalculateAverageRulerLeaderPips(tradition, militarySkill));
            await ReplyAsync($"Average ruler general pips: Fire {leaderPipResult.Fire}, Shock {leaderPipResult.Shock}, Maneuver {leaderPipResult.Maneuver}, Siege {leaderPipResult.Siege}");
        }
    }
}
