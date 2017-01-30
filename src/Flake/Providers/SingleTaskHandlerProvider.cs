using System;
using Flame.Compiler;

namespace Flake.Providers
{
    /// <summary>
    /// A command provider that can only provide one type of
    /// command.
    /// </summary>
    public sealed class SingleTaskHandlerProvider : ITaskHandlerProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Providers.SingleTaskHandlerProvider"/> class.
        /// </summary>
        /// <param name="Handler">The task handler.</param>
        public SingleTaskHandlerProvider(ITaskHandler Handler)
        {
            this.Handler = Handler;
        }

        /// <summary>
        /// Gets the command that is provider wraps.
        /// </summary>
        /// <value>The command.</value>
        public ITaskHandler Handler { get; private set; }

        /// <inheritdoc/>
        public ResultOrError<ITaskHandler, LogEntry> GetHandler(
            TaskDescription Description, ICompilerLog Log)
        {
            if (Handler.TaskType == Description.Type)
                return ResultOrError<ITaskHandler, LogEntry>.CreateResult(Handler);
            else
                return EmptyTaskHandlerProvider.Instance.GetHandler(Description, Log);
        }
    }
}

