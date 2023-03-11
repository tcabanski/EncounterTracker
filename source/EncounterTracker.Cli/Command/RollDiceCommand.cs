using System.Text;
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
        var sb = new StringBuilder();
        foreach (var roll in result.Values)
        {
            if (roll.DieType == DieType.Normal)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(roll.Value);
            }
        }
        Console.WriteLine($"Result {result.Expression} ({sb})");
        Console.WriteLine($"Total: {result.Value}");
        return 0;
    }
}