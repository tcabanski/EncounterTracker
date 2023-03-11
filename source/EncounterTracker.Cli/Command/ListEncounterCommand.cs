using Spectre.Console;
using Spectre.Console.Cli;

namespace EncounterTracker.Cli.Command;

public class ListEncounterCommand : AsyncCommand<ListEncounterCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var table = new Table().LeftAligned();

        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                table.AddColumn("Foo");
                ctx.Refresh();
                await Task.Delay(1000);

                table.AddColumn("Bar");
                ctx.Refresh();
                await Task.Delay(1000);
            });

        return 0;
    }
}