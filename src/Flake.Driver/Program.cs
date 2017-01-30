using System;
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
                log.LogEvent(new LogEntry("usage", "flake command [args...]"));
                return;
            }

            string commandName = Args[0];
        }
    }
}
