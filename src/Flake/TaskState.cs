using System;
using System.Collections.Generic;

namespace Flake
{
    /// <summary>
    /// A read-only view of the state tasks need to execute.
    /// </summary>
    public struct TaskState
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Flake.TaskState"/> struct.
        /// </summary>
        /// <param name="StateBuilder">
        /// The state builder of which a read-only view is created.
        /// </param>
        public TaskState(TaskStateBuilder StateBuilder)
        {
            this.stateBuilder = StateBuilder;
        }

        private TaskStateBuilder stateBuilder;

        /// <summary>
        /// Gets the result for the task with the given identifier.
        /// </summary>
        /// <returns>The task result.</returns>
        /// <param name="Identifier">The task identifier.</param>
        public TaskResult GetResult(TaskIdentifier Identifier)
        {
            return stateBuilder.GetResult(Identifier);
        }
    }
}

