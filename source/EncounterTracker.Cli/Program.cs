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
                c.Settings.CaseSensitivity = CaseSensitivity.None;
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
            
            //syntax is c <min> or c <min>:<max>
            [CommandOption("-c|--challenge-rating")]
            public string? Cr { get; set; }
            [CommandOption("--sortCr")]
            public bool SortCr { get; set; }
            
            [CommandOption("--sortDesc")]
            public bool SortDesc { get; set; }


        }


        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            try
            {
                var creaturesSearch = Program.Container.Resolve<IDocumentStore>().OpenSession().Query<Creature>()
                    .Search(x => x.Name, settings.Name);
                if (settings.Cr != null)
                {
                    var cr = settings.Cr.Split(':');
                    creaturesSearch = cr.Length == 2 ? creaturesSearch.Filter(x => x.Challenge >= double.Parse(cr[0]) && x.Challenge <= double.Parse(cr[1])) : 
                        creaturesSearch.Filter(x => x.Challenge >= double.Parse(settings.Cr));
                }

                IList<Creature> creatures = new List<Creature>();
                if (settings.SortCr)
                {
                    if (settings.SortDesc)
                    {
                        creatures = creaturesSearch.OrderByDescending(x => x.Challenge).ToList();
                    }
                    else
                    {
                        creatures = creaturesSearch.OrderBy(x => x.Challenge).ToList();
                    }
                }
                else
                {
                    if (settings.SortDesc)
                    {
                        creatures = creaturesSearch.OrderByDescending(x => x.Name).ToList();
                    }
                    else
                    {
                        creatures = creaturesSearch.OrderBy(x => x.Name).ToList();
                    }
                }
                AnsiConsole.MarkupLine($"Found {creatures.Count} creatures");
                for (int x = 0; x < creatures.Count; x++)
                {
                    AnsiConsole.MarkupLine($"[bold red]{x}[/] CR: {creatures[x].Challenge} {creatures[x].Name}");
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