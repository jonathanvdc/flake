using System;
using System.Collections.Generic;

namespace Flake
{
    /// <summary>
    /// Represents a task: a single, directly runnable piece of a project.
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Gets this task's unique identifier.
        /// </summary>
        /// <value>The identifier.</value>
        TaskIdentifier Identifier { get; }

        /// <summary>
        /// Gets this task's list of dependencies.
        /// </summary>
        /// <value>This task's dependencies.</value>
        IReadOnlyList<ITask> Dependencies { get; }
    }
}

