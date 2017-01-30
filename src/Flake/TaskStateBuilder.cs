using System;
using System.Collections.Generic;

namespace Flake
{
    /// <summary>
    /// A mutable data structure that remembers which tasks have been
    /// completed.
    /// </summary>
    public sealed class TaskStateBuilder
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Flake.TaskStateBuilder"/> class.
        /// </summary>
        public TaskStateBuilder()
        {
            this.results = new Dictionary<TaskIdentifier, TaskResult>();
        }

        private Dictionary<TaskIdentifier, TaskResult> results;

        /// <summary>
        /// Completes the task with the given identifier, and associates
        /// it with the given result.
        /// </summary>
        /// <param name="Identifier">The task's identifier.</param>
        /// <param name="Result">The task's result.</param>
        public void Complete(TaskIdentifier Identifier, TaskResult Result)
        {
            results[Identifier] = Result;
        }

        /// <summary>
        /// Determines if the task with the given identifier has been 
        /// completed yet.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the task with the given identifier has been 
        /// completed yet; otherwise, <c>false</c>.</returns>
        /// <param name="Identifier">The task's identifier.</param>
        public bool IsCompleted(TaskIdentifier Identifier)
        {
            return results.ContainsKey(Identifier);
        }

        /// <summary>
        /// Gets the result for the task with the given identifier.
        /// </summary>
        /// <returns>The task result.</returns>
        /// <param name="Identifier">The task identifier.</param>
        public TaskResult GetResult(TaskIdentifier Identifier)
        {
            return results[Identifier];
        }
    }
}

