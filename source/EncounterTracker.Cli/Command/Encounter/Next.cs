using Spectre.Console;
using Spectre.Console.Cli;

namespace EncounterTracker.Cli.Command.Encounter;

public class Next : AsyncCommand<Next.Settings>
{
    public class Settings : CommandSettings
    {
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Console.WriteLine("Next turn");
        return 0;
    }
}