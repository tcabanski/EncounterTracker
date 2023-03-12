using Discord;
using Discord.WebSocket;
using NLog;

namespace EncounterTracker.Shared.DiscordClient
{
    public class DiscordClientHolder
    {
        private static DiscordSocketClient discordClient;
        public static ILogger Logger;

        private static async Task<DiscordSocketClient> CreateDiscordClient()
        {
            Logger = LogManager.GetCurrentClassLogger();
            discordClient = new DiscordSocketClient();

            discordClient.Log += Log;

            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");


            await discordClient.LoginAsync(Discord.TokenType.Bot, token);
            await discordClient.StartAsync();

            return discordClient;
        }

        public static DiscordSocketClient DiscordClient
        {
            get
            {
                if (discordClient == null)
                {
                    CreateDiscordClient().Wait();
                }

                return discordClient;
            }
        }

        static Task Log(LogMessage msg)
        {
            Logger.Info(msg.ToString());
            return Task.CompletedTask;
        }
    }

    
}
