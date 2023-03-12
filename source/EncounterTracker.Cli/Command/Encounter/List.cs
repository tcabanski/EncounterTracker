using Spectre.Console;
using Spectre.Console.Cli;

namespace EncounterTracker.Cli.Command.Encounter;

public class List : AsyncCommand<List.Settings>
{
    public class Settings : CommandSettings
    {
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Console.WriteLine("List encounters");
        return 0;
    }
}