using Spectre.Console;
using Spectre.Console.Cli;

namespace EncounterTracker.Cli.Command.Encounter;

public class End : AsyncCommand<End.Settings>
{
    public class Settings : CommandSettings
    {
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Console.WriteLine("End encounter");
        return 0;
    }
}