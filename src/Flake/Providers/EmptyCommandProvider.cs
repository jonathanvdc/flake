using System;
using Flame.Compiler;

namespace Flake.Providers
{
    /// <summary>
    /// A command provider that never manages to actually resolve
    /// a command.
    /// </summary>
    public sealed class EmptyCommandProvider : ICommandProvider
    {
        private EmptyCommandProvider()
        { }

        /// <summary>
        /// The canonical instance of the empty command provider.
        /// </summary>
        public static readonly EmptyCommandProvider Instance =
            new EmptyCommandProvider();

        /// <inheritdoc/>
        public ResultOrError<ICommand, LogEntry> GetCommand(string Name)
        {
            return ResultOrError<ICommand, LogEntry>.CreateError(
                new LogEntry(
                    "unknown command type",
                    "cannot resolve command type '" + Name + "'."));
        }
    }
}

