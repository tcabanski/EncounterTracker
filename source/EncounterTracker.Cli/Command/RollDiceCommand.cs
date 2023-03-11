using System.Text;
using System.Text.RegularExpressions;
using Autofac;
using Dice;
using EncounterTracker.Shared.FifthEdition;
using Raven.Client.Documents;
using Spectre.Console;
using Spectre.Console.Cli;

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
        AnsiConsole.MarkupLine($"{HighlightCritsForConsole(result.ToString())}");
        return 0;
    }

    public string HighlightCritsForConsole(string input)
    {
        string replacement = "replacement_text";

        bool foundMatches;
        do
        {
            foundMatches = false;
            MatchCollection matches = Regex.Matches(input, @"\d+!");
            if (matches.Count > 0)
            {
                input = input.Replace(matches[0].Value, "[bold red]" + matches[0].Value.TrimEnd('!') + "[/]");
                foundMatches = true;
            }
        } while (foundMatches);

        return input;
    }
}