using System;
using System.Linq;
using Flame.Front;
using Flame.Front.Cli;
using Flame.Compiler;

namespace Flake.Driver
{
    public static class Program
    {
        public static void Main(string[] Args)
        {
            // Acquire a console log.
            var logOptions = EmptyCompilerOptions.Instance;
            var log = new ConsoleLog(
                ConsoleEnvironment.AcquireConsole(logOptions),
                logOptions);

            if (Args.Length == 0)
            {
                log.LogEvent(new LogEntry("usage", "flake [file|--] command [args...]"));
                return;
            }

            string commandOrFilename = Args[0];
            string maybeCommand = Args.Length >= 2 ? Args[1] : null;
            bool isTwoWordCommand;

            var commandParser = new CommandParser(
                Flake.Providers.EmptyCommandProvider.Instance, 
                Flake.Providers.EmptyTaskHandlerProvider.Instance,
                log);
            
            var commandSpecResult = commandParser.ParseCommandSpec(
                commandOrFilename, maybeCommand, out isTwoWordCommand);

            if (commandSpecResult.IsError)
            {
                log.LogError(commandSpecResult.ErrorOrDefault);
                return;
            }
            else
            {
                commandSpecResult.ResultOrDefault.Run(
                    Args.Skip(isTwoWordCommand ? 2 : 1).ToArray(),
                    log);
            }
        }
    }
}
