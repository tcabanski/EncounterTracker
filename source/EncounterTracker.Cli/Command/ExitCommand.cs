using Spectre.Console.Cli;

namespace EncounterTracker.Cli.Command;

public class ExitCommand : AsyncCommand<ExitCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Program.IsExitCommandIssued = true;
        return 0;
    }
}