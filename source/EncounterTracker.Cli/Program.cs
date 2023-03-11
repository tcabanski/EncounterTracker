using System.Collections;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Dice.Grammar;
using EncounterTracker.Shared.Base;
using Autofac;
using Spectre.Console.Cli;
using IContainer = Autofac.IContainer;
using EncounterTracker.Cli.Command;

namespace EncounterTracker.Cli
{
    public class Program
    {
        public static IContainer Container;
        public static bool IsExitCommandIssued = false;
        static async Task<int> Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
            Container = ConfigureContainer();

            var app = new CommandApp();
            app.Configure(c =>
            {
                c.Settings.CaseSensitivity = CaseSensitivity.None;
                c.Settings.ApplicationName = "";
                c.AddBranch("creature", creature =>
                {
                    creature.AddCommand<ListCreatureCommand>("list");
                }).WithAlias("c");
                c.AddBranch("encounter", creature =>
                {
                    creature.AddCommand<ListEncounterCommand>("list");
                }).WithAlias("e");
                c.AddCommand<RollDiceCommand>("roll").WithAlias("r");
                c.AddCommand<ExitCommand>("exit");
            });

            while (!IsExitCommandIssued)
            {
                await app.RunAsync(args);
                if (!IsExitCommandIssued)
                {
                    Console.WriteLine();
                    Console.Write("Command>");
                    var input = Console.ReadLine();
                    args = CommandLineStringSplitter.Instance.Split(input.Trim()).ToArray();
                }
            }

            return 0;

        }

        static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(EncounterTracker.Data.Registrar).Assembly);

            return builder.Build();
        }
    }
}