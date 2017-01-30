using System;
using Flame.Compiler;

namespace Flake.Providers
{
    /// <summary>
    /// A task handler provider that never manages to actually resolve
    /// a task handler.
    /// </summary>
    public sealed class EmptyTaskHandlerProvider : ITaskHandlerProvider
    {
        private EmptyTaskHandlerProvider()
        { }

        /// <summary>
        /// The canonical instance of the empty task handler provider.
        /// </summary>
        public static readonly EmptyTaskHandlerProvider Instance =
            new EmptyTaskHandlerProvider();

        /// <inheritdoc/>
        public ResultOrError<ITaskHandler, LogEntry> GetHandler(
            TaskDescription Description, ICompilerLog Log)
        {
            return ResultOrError<ITaskHandler, LogEntry>.CreateError(
                new LogEntry(
                    "unknown task type",
                    "cannot resolve task type '" + Description.ToString() + "'."));
        }
    }
}

