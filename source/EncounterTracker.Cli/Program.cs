using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace EncounterTracker.Cli
{
    using Autofac;
    using Spectre.Console;
    using Spectre.Console.Cli;

    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var container = ConfigureContainer();

            var app = new CommandApp();
            app.Configure(c =>
            {
                c.AddBranch("creature", creature =>
                {
                    creature.AddCommand<ListCreatureCommand>("list");
                });
                c.AddBranch("encounter", creature =>
                {
                    creature.AddCommand<ListEncounterCommand>("list");
                });

            });

            return await app.RunAsync(args);

        }

        static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(EncounterTracker.Data.Registrar).Assembly);

            return builder.Build();
        }
    }

    public class ListCreatureCommand : AsyncCommand<ListCreatureCommand.Settings>
    {
        public class Settings : CommandSettings
        {
        }


        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsole.MarkupLine("[blue]list creature[/]");
            return 0;
        }
    }

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
}