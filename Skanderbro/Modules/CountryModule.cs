using System.Threading.Tasks;
using Discord.Commands;
using Skanderbro.Models;
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
            CountryTagResult result = await countryTagService.GetCountryTag(countryName);
            if (result != null)
            {
                if (result.LevenshteinDistance == 0)
                {
                    await ReplyAsync(result.Tag);
                }
                else
                {
                    await ReplyAsync($"Country named {countryName} was not found. The closest available country is {result.Name} with tag {result.Tag}.");
                }
            }
            else
            {
                await ReplyAsync($"Could not fetch country tags.");
            }
        }
    }
}
