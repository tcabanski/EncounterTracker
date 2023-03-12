using Spectre.Console;
using Spectre.Console.Cli;

namespace EncounterTracker.Cli.Command.Encounter;

public class Save : AsyncCommand<Save.Settings>
{
    public class Settings : CommandSettings
    {
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Console.WriteLine("Save encounter");
        return 0;
    }
}