using Spectre.Console;
using Spectre.Console.Cli;

namespace EncounterTracker.Cli.Command.Encounter;

public class Start : AsyncCommand<Start.Settings>
{
    public class Settings : CommandSettings
    {
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Console.WriteLine("Start encounter");
        return 0;
    }
}