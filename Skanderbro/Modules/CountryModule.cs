using System.Threading.Tasks;
using Discord.Commands;
using Skanderbro.Services;

namespace Skanderbro.Modules
{
    public sealed class CountryModule : ModuleBase<SocketCommandContext>
    {
        private readonly ICountryTagService countryTagService;

        public CountryModule(ICountryTagService countryTagService)
        {
            this.countryTagService = countryTagService;
        }

        // !tag Bohemia -> BOH
        [Command("tag")]
        [Summary("Get the tag for a given non-custom nation.")]
        public async Task GetCountryTagAsync([Summary("The name of the country whose tag to search")] string countryName)
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
