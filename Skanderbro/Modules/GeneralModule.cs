using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Skanderbro.Services;

namespace Skanderbro.Modules
{
    public sealed class GeneralModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService commandService;
        private readonly ICountryTagService countryTagService;
        private readonly ILeaderPipService leaderPipService;

        public GeneralModule(CommandService commandService, ICountryTagService countryTagService, ILeaderPipService leaderPipService)
        {
            this.commandService = commandService;
            this.countryTagService = countryTagService;
            this.leaderPipService = leaderPipService;
        }

        // !say hello world -> hello world
        [Command("say")]
        [Summary("Assert the dominance of man over machine by making Skanderbro say something silly.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        {
            return ReplyAsync(echo);
        }

        [Command("commands")]
        [Summary("Gets the list of all the tricks Skanderbro was taught using positive reinforcement methods.")]
        public async Task ListCommandsAsync()
        {
            var commands = commandService.Commands.ToList();
            var embedBuilder = new EmbedBuilder();

            foreach (CommandInfo command in commands)
            {
                // Get the command Summary attribute information
                string embedFieldText = command.Summary ?? "No description available\n";

                embedBuilder.AddField($"!{command.Name}", embedFieldText);
            }

            await ReplyAsync("Here's a list of commands and their descriptions: ", false, embedBuilder.Build());
        }

        [Command("help")]
        [Summary("Reminds you how to use a certain command (since you keep forgetting).")]
        public async Task GetCommandHelpAsync([Summary("Name of the command you need help with")] string commandName)
        {
            var commands = commandService.Commands.ToList();
            var searchedCommand = commands.Find(c => c.Name == commandName);
            if (searchedCommand == null)
            {
                await ReplyAsync($"Command named {commandName} not found... check your spelling buster.");
            }
            else
            {
                var builder = new EmbedBuilder
                {
                    Title = searchedCommand.Summary
                };
                if (searchedCommand.Parameters.Count > 0)
                {
                    builder.Description = "Parameter list:";
                    foreach (var parameter in searchedCommand.Parameters)
                    {
                        builder.AddField(parameter.Name + $" ({parameter.Type.Name})" + (parameter.IsOptional ? " [optional]" : ""), parameter.Summary);
                    }
                }
                else
                {
                    builder.Description = "No parameters.";
                }

                await ReplyAsync($"Showing help for {searchedCommand.Name}:", false, builder.Build());
            }
        }
    }
}
