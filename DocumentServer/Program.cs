using ComponentModelRPC.Server;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TVP.CollaborativeEditor.Models;

namespace DocumentServer
{
    class Program
    {

        static void ConfigureLogger()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget("console")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);
            LogManager.Configuration = config;
        }

        static void Main(string[] args)
        {
            ConfigureLogger();
            var host = new ServerHost() { ListenPort = 8120 };
            Console.Title = $"Document host on port {host.ListenPort}";
            host.Initialize(AppDataContext.Instance.Document, address => new GenericPrincipal(new GenericIdentity("A user name"), new string[0]));
            while (true)
            {
                Console.Write('>');
                var line = Console.ReadLine();
                var lineParts = line?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (lineParts?.Length >= 1)
                    switch (lineParts[0])
                    {
                        case "quit":
                            return;
                    }
            }
        }
    }
}
