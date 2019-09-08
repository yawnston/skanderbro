using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Serilog;
using Skanderbro.Configuration;

namespace Skanderbro
{
    public sealed class SkanderbroBot
    {
        private readonly DiscordSocketClient client;
        private readonly string token;
        private readonly ILogger logger;

        public SkanderbroBot(DiscordSocketClient client, IOptions<BotSecrets> botSecrets, ILogger logger)
        {
            this.client = client;
            this.client.Log += Client_Log;
            token = botSecrets.Value.SkanderbroClientSecret;
            this.logger = logger;
        }

        public async Task Run()
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(Timeout.Infinite);
        }

        private Task Client_Log(LogMessage arg)
        {
            logger.Information(arg.Message);
            return Task.CompletedTask;
        }
    }
}
