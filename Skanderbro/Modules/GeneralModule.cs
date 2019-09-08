using System.Threading.Tasks;
using Discord.Commands;
using Skanderbro.Services;

namespace Skanderbro.Modules
{
    public sealed class GeneralModule : ModuleBase<SocketCommandContext>
    {
        private readonly ICountryTagService countryTagService;

        public GeneralModule(ICountryTagService countryTagService)
        {
            this.countryTagService = countryTagService;
        }

        // !say hello world -> hello world
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
            => ReplyAsync(echo);

        // !tag Bohemia -> BOH
        [Command("tag")]
        [Summary("Get the tag for a given non-custom nation")]
        public async Task GetCountryTagAsync([Remainder] [Summary("The name of the country whose tag to search")] string countryName)
        {
            var tag = await countryTagService.GetCountryTag(countryName);
            if (tag != null)
            {
                await ReplyAsync(tag);
            }
            else
            {
                await ReplyAsync($"Country {countryName} was not found.");
            }
        }
    }
}
