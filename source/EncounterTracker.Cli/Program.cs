namespace EncounterTracker.Cli
{
    using Autofac;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var container = ConfigureContainer();

            var fileOption = new Option<FileInfo?>(
                name: "--file",
                description: "The file to read and display on the console.")
            { IsRequired = true };

            var delayOption = new Option<int>(
                name: "--delay",
                description: "Delay between lines, specified as milliseconds per character in a line.",
                getDefaultValue: () => 42);

            var fgcolorOption = new Option<ConsoleColor>(
                name: "--fgcolor",
                description: "Foreground color of text displayed on the console.",
                getDefaultValue: () => ConsoleColor.White);

            var lightModeOption = new Option<bool>(
                name: "--light-mode",
                description: "Background color of text displayed on the console: default is black, light mode is white.");

            var encounterCommand = new Command("encounter", "Work with encounters.");
            var displayCreatureCommand = new Command("display", "Display a creature.");

            var creatureCommand = new Command("creature", "Work with creatures.")
            {
                displayCreatureCommand
            };

            displayCreatureCommand.SetHandler(HandleCreatureDisplay);
            encounterCommand.SetHandler(HandleEncounter);
            var rootCommand = new RootCommand("Encounter Tracker")
            {
                creatureCommand,
                encounterCommand
            };

            return await rootCommand.InvokeAsync(args);
        }

        static async Task HandleCreatureDisplay()
        {
            Console.WriteLine("creature display");
        }

        static async Task HandleEncounter()
        {
            Console.WriteLine("encounter");
        }

        static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(EncounterTracker.Data.Registrar).Assembly);

            return builder.Build();
        }
    }
}