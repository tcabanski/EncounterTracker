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

namespace EncounterTracker.Cli
{
    public class Program
    {
        public static IContainer Container;
        public static bool IsExitCommandIssued = false;
        public static DiscordSocketClient DiscordClient;
        public static ILogger Logger;
        public static bool IsDiscordConnected;
        public static ISocketMessageChannel DiscordChannel;
        static async Task<int> Main(string[] args)
        {
            Logger = LogManager.GetCurrentClassLogger();
            LoadEnv(".env");
            Container = ConfigureContainer();
            DiscordClient = new DiscordSocketClient();

            DiscordClient.Log += Log;

            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");


            await DiscordClient.LoginAsync(Discord.TokenType.Bot, token);
            await DiscordClient.StartAsync();

            var app = new CommandApp();
            app.Configure(c =>
            {
                c.Settings.CaseSensitivity = CaseSensitivity.None;
                c.Settings.ApplicationName = "";
                c.AddCommand<DiscordCommand>("discord").WithAlias("d").WithData(DiscordClient);
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
                await DiscordClient.StopAsync();
            }

            return 0;

        }

        static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(EncounterTracker.Data.Registrar).Assembly);

            return builder.Build();
        }

        static Task Log(LogMessage msg)
        {
            Logger.Info(msg.ToString());
            return Task.CompletedTask;
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