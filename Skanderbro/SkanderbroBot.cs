using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Skanderbro.Configuration;

namespace Skanderbro
{
    public sealed class SkanderbroBot
    {
        private readonly DiscordSocketClient client;

        public SkanderbroBot(IOptions<BotSecrets> botSecrets)
        {
            client = new DiscordSocketClient();
            client.Log += Client_Log;
            string token = botSecrets.Value.SkanderbroClientSecret;
        }

        private Task Client_Log(LogMessage arg)
        {
            throw new System.NotImplementedException();
        }
    }
}
