using Autofac;
using EncounterTracker.Shared.FifthEdition;
using Raven.Client.Documents;
using Spectre.Console;
using Spectre.Console.Cli;

namespace EncounterTracker.Cli.Command.Creature;

public class List : AsyncCommand<List.Settings>
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
            var creaturesSearch = Program.Container.Resolve<IDocumentStore>().OpenSession().Query<Shared.FifthEdition.Creature>()
                .Search(x => x.Name, settings.Name);
            if (settings.Cr != null)
            {
                var cr = settings.Cr.Split(':');
                creaturesSearch = cr.Length == 2 ? creaturesSearch.Filter(x => x.Challenge >= double.Parse(cr[0]) && x.Challenge <= double.Parse(cr[1])) :
                    creaturesSearch.Filter(x => x.Challenge >= double.Parse(settings.Cr));
            }

            IList<Shared.FifthEdition.Creature> creatures = new List<Shared.FifthEdition.Creature>();
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