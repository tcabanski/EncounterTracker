using System.Text;
using System.Text.RegularExpressions;
using Autofac;
using Dice;
using Discord.WebSocket;
using EncounterTracker.Shared.FifthEdition;
using Raven.Client.Documents;
using Spectre.Console;
using Spectre.Console.Cli;
using TextCopy;

namespace EncounterTracker.Cli.Command;

public class DiscordCommand : AsyncCommand<DiscordCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ChannelName>")]
        public string ChannelName { get; set; }
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var discordClient = (context.Data as DiscordSocketClient);
        // Get the guild that the channel is in by its name
        var guild = discordClient.Guilds.FirstOrDefault();

        // Get the channel by its name
        var channel = guild?.Channels.FirstOrDefault(c => c.Name == settings.ChannelName) as ISocketMessageChannel;

        if (channel != null)
        {
            await channel.SendMessageAsync("Encounter tracker connected.");
            Program.IsDiscordConnected = true;
            Program.DiscordChannel = channel;
        }
        return 0;
    }
}