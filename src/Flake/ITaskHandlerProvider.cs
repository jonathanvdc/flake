using System;
using Flame.Compiler;

namespace Flake
{
    /// <summary>
    /// Defines common functionality for objects that create
    /// task handlers based on their task types. 
    /// </summary>
    public interface ITaskHandlerProvider
    {
        /// <summary>
        /// Tries to find a handler for the given type of task.
        /// </summary>
        /// <param name="Description">The task type's description</param>
        /// <param name="Log">The log to which diagnostics may be sent.</param>
        /// <returns>The task handler, or an error.</returns>
        ResultOrError<ITaskHandler, LogEntry> GetHandler(
            TaskDescription Description, ICompilerLog Log);
    }
}

