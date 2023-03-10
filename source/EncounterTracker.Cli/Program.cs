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
using Discord;
using Discord.WebSocket;
using System.Text;
using NLog;
using EncounterTracker.Data;
using EncounterTracker.Shared.DiscordClient;
using Raven.Client.Documents;

namespace EncounterTracker.Cli
{
    public class Program
    {
        public static IContainer Container;
        public static bool IsExitCommandIssued = false;
        public static ILogger Logger;
        public static bool IsDiscordConnected;
        public static ISocketMessageChannel DiscordChannel;
        static async Task<int> Main(string[] args)
        {
            Logger = LogManager.GetCurrentClassLogger();
            LoadEnv(".env");
            Container = ConfigureContainer();
            
            //TODO: ideally, we'd be using DI here, but the Spectre library assumes RunAsync will be called only once.
            //Therefore, their DI support is broken when looping like this app does.
            //At some point it would be nice to fix that.
            var app = new CommandApp();
            app.Configure(c =>
            {
                c.Settings.CaseSensitivity = CaseSensitivity.None;
                c.Settings.ApplicationName = "";
                c.AddCommand<DiscordCommand>("discord").WithAlias("d").WithData(Container.Resolve<DiscordSocketClient>());
                c.AddBranch("creature", creature =>
                {
                    creature.AddCommand<EncounterTracker.Cli.Command.Creature.List>("list");
                }).WithAlias("c");
                c.AddBranch("encounter", encounter =>
                {
                    encounter.AddCommand<EncounterTracker.Cli.Command.Encounter.List>("list");
                    encounter.AddCommand<EncounterTracker.Cli.Command.Encounter.Load>("load");
                    encounter.AddCommand<EncounterTracker.Cli.Command.Encounter.Save>("save");
                    encounter.AddCommand<EncounterTracker.Cli.Command.Encounter.Start>("start");
                    encounter.AddCommand<EncounterTracker.Cli.Command.Encounter.Next>("next");
                    encounter.AddCommand<EncounterTracker.Cli.Command.Encounter.End>("end");
                }).WithAlias("e");
                c.AddCommand<RollDiceCommand>("roll").WithAlias("r");
                c.AddCommand<ExitCommand>("exit");
            });

           
            try
            {

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
            }
            finally
            {
                if (IsDiscordConnected)
                {
                    await DiscordChannel.SendMessageAsync("Encounter tracker disconnected.");
                }
                await Container.Resolve<DiscordSocketClient>().StopAsync();
            }

            return 0;

        }

        static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(EncounterTracker.Data.Registrar).Assembly);
            builder.Register(c => DiscordClientHolder.DiscordClient).As<DiscordSocketClient>().SingleInstance();

            return builder.Build();
        }

        static void LoadEnv(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}