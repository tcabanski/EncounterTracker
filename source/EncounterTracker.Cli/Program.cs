using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Dice.Grammar;
using EncounterTracker.Shared.Base;
using EncounterTracker.Shared.FifthEdition;
using Raven.Client.Documents;

namespace EncounterTracker.Cli
{
    using Autofac;
    using Spectre.Console;
    using Spectre.Console.Cli;

    public class Program
    {
        public static IContainer Container;
        static async Task<int> Main(string[] args)
        {
            Container = ConfigureContainer();

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
            [CommandArgument(0, "<Name>")]
            public string Name { get; set; }
        }


        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            try
            {
                var creatures = Program.Container.Resolve<IDocumentStore>().OpenSession().Query<Creature>().Search(x => x.Name, settings.Name).ToList();
                AnsiConsole.MarkupLine($"Found {creatures.Count} creatures");
                for (int x = 0; x < creatures.Count; x++)
                {
                    var highlightedName = creatures[x].Name.Replace(settings.Name, $"[yellow]{settings.Name}[/]",
                        StringComparison.OrdinalIgnoreCase);
                    AnsiConsole.MarkupLine($"[bold red]{x}[/] {highlightedName}");
                }

                return 0;
            }
            catch (Exception err)
            {
                AnsiConsole.WriteLine($"Error: {err}");
                return 1;
            }
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