using System;
using System.Collections.Generic;
using Flake.Extensibility;
using Flame.Compiler;

namespace Flake.Echo
{
    /// <summary>
    /// A command that prints a line of text.
    /// </summary>
    [FlakeImport]
    public sealed class EchoCommand : ICommand
    {
        private EchoCommand()
        { }

        /// <summary>
        /// The canonical instance of the echo command.
        /// </summary>
        [FlakeImport]
        public static readonly EchoCommand Instance = new EchoCommand();

        /// <inheritdoc/>
        public string Name { get { return "echo"; } }

        /// <inheritdoc/>
        public void Run(IReadOnlyList<string> Arguments, ICompilerLog Log)
        {
            Log.LogEvent(new LogEntry("Status", string.Join(" ", Arguments)));
        }
    }
}

