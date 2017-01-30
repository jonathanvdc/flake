using System;
using System.Collections.Generic;
using Flame.Compiler;

namespace Flake
{
    /// <summary>
    /// Represents a task: a single, directly runnable piece of a project.
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Gets this task's list of dependencies.
        /// </summary>
        /// <value>This task's dependencies.</value>
        IReadOnlyList<TaskIdentifier> Dependencies { get; }

        /// <summary>
        /// Runs this task.
        /// </summary>
        /// <param name="State">The state of the task environment.</param>
        /// <param name="Log">The log to which diagnostics may be sent.</param>
        /// <returns>Either a task result, or an error diagnostic.</returns>
        ResultOrError<TaskResult, LogEntry> Run(TaskState State, ICompilerLog Log);
    }
}

