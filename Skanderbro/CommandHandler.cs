using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Skanderbro.Models;
using Skanderbro.TypeReaders;

namespace Skanderbro
{
    public sealed class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly IServiceProvider services;

        public CommandHandler(IServiceProvider services, CommandService commands, DiscordSocketClient client)
        {
            this.commands = commands;
            this.services = services;
            this.client = client;
        }

        public async Task InitializeAsync()
        {
            commands.AddTypeReader<LeaderPipModifiers>(new LeaderPipModifiersTypeReader());
            // Pass the service provider to the second parameter of
            // AddModulesAsync to inject dependencies to all modules 
            // that may require them.
            var c = await commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: services);
            commands.CommandExecuted += OnCommandExecutedAsync;
            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (!(messageParam is SocketUserMessage message)) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos)
                || message.HasMentionPrefix(client.CurrentUser, ref argPos))
                || message.Author.IsBot)
            {
                return;
            }

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: services);
        }

        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // We have access to the information of the command executed,
            // the context of the command, and the result returned from the
            // execution in this event.

            // We can tell the user what went wrong
            if (!string.IsNullOrEmpty(result?.ErrorReason))
            {
                string responseMessage = result.ErrorReason;
                if (command.IsSpecified)
                {
                    responseMessage += $" Type `!help {command.Value.Name}` to get Skanderbro to help with this command.";
                }
                else
                {
                    responseMessage += $" Type `!commands` to find out what Skanderbro can do.";
                }
                await context.Channel.SendMessageAsync(responseMessage);
            }
        }
    }
}
