using System;
using Flame.Compiler;

namespace Flake.Providers
{
    /// <summary>
    /// A command provider that can only provide one type of
    /// command.
    /// </summary>
    public sealed class SingleCommandProvider : ICommandProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Providers.SingleCommandProvider"/> class.
        /// </summary>
        /// <param name="Command">The command to provide.</param>
        public SingleCommandProvider(ICommand Command)
        {
            this.Command = Command;
        }

        /// <summary>
        /// Gets the command that is provider wraps.
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command { get; private set; }

        /// <inheritdoc/>
        public ResultOrError<ICommand, LogEntry> GetCommand(
            string Name, ICompilerLog Log)
        {
            if (Command.Name == Name)
                return ResultOrError<ICommand, LogEntry>.CreateResult(Command);
            else
                return EmptyCommandProvider.Instance.GetCommand(Name, Log);
        }
    }
}

