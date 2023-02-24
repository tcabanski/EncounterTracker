namespace EncounterTracker.Cli
{
    using System.CommandLine;
    
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
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

            var rootCommand = new RootCommand("Sample app for System.CommandLine");
            
            var readCommand = new Command("read", "Read and display the file.")
            {
                fileOption,
                delayOption,
                fgcolorOption,
                lightModeOption
            };
            rootCommand.AddCommand(readCommand);

            readCommand.SetHandler(async (file, delay, fgcolor, lightMode) =>
                {
                    await ReadFile(file!, delay, fgcolor, lightMode);
                },
                fileOption, delayOption, fgcolorOption, lightModeOption);

            return await rootCommand.InvokeAsync(args);
        }

        internal static async Task ReadFile(
            FileInfo file, int delay, ConsoleColor fgColor, bool lightMode)
        {
            Console.BackgroundColor = lightMode ? ConsoleColor.White : ConsoleColor.Black;
            Console.ForegroundColor = fgColor;
            List<string> lines = File.ReadLines(file.FullName).ToList();
            foreach (string line in lines)
            {
                Console.WriteLine(line);
                await Task.Delay(0 * line.Length);
            };
        }
    }
}