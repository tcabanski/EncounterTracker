using System.Text;
using System.Text.RegularExpressions;
using Autofac;
using Dice;
using EncounterTracker.Shared.FifthEdition;
using Raven.Client.Documents;
using Spectre.Console;
using Spectre.Console.Cli;
using TextCopy;

namespace EncounterTracker.Cli.Command;

public class RollDiceCommand : AsyncCommand<RollDiceCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--private|-p")]
        public bool Private { get; set; }
        [CommandArgument(0, "<Dice Expression (see https://skizzerz.net/DiceRoller/Dice_Reference)>")]
        public string DiceExpression { get; set; }
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var result = Roller.Roll(settings.DiceExpression);
        AnsiConsole.MarkupLine(StrikeThroughDiscardedDice(HighlightCrits(result.ToString(), "[bold red]", "[/]"), "[strikethrough]", "[/]"));

        string markdownText = HighlightCrits(StrikeThroughDiscardedDice(result.ToString(), "~~", "~~"), "**", "**");
        await ClipboardService.SetTextAsync(markdownText);
        if (Program.IsDiscordConnected)
        {
            await Program.DiscordChannel.SendMessageAsync(markdownText);
        }
        return 0;
    }

    public string HighlightCrits(string input, string replaceFront, string replaceBack)
    {
        bool foundMatches;
        do
        {
            foundMatches = false;
            MatchCollection matches = Regex.Matches(input, @"\d+!");
            if (matches.Count > 0)
            {
                input = input.Replace(matches[0].Value, replaceFront + matches[0].Value.TrimEnd('!') + replaceBack);
                foundMatches = true;
            }
        } while (foundMatches);

        return input;
    }

    public string StrikeThroughDiscardedDice(string input, string replaceFront, string replaceBack)
    {
        bool foundMatches;
        do
        {
            foundMatches = false;
            MatchCollection matches = Regex.Matches(input, @"\d+\*{1}");
            if (matches.Count > 0)
            {
                input = input.Replace(matches[0].Value, replaceFront + matches[0].Value.TrimEnd('*') + replaceBack);
                foundMatches = true;
            }
        } while (foundMatches);

        return input;
    }
}