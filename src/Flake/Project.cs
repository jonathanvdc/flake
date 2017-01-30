using System;
using System.Collections.Generic;

namespace Flake
{
    /// <summary>
    /// Defines a project: a collection of tasks.
    /// </summary>
    public sealed class Project
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Project"/> class.
        /// </summary>
        /// <param name="Identifier">The project's identifier.</param>
        /// <param name="Tasks">The project's list of tasks.</param>
        public Project(
            ProjectIdentifier Identifier, 
            IReadOnlyDictionary<string, ITask> Tasks)
        {
            this.Identifier = Identifier;
            this.Tasks = Tasks;
        }

        /// <summary>
        /// Gets this project's identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public ProjectIdentifier Identifier { get; private set; }

        /// <summary>
        /// Gets this project's list of tasks.
        /// </summary>
        /// <value>The list of tasks.</value>
        public IReadOnlyDictionary<string, ITask> Tasks { get; private set; }

        /// <summary>
        /// Tries to get the task with the given name.
        /// </summary>
        /// <returns><c>true</c>, if a task with the given name was found, <c>false</c> otherwise.</returns>
        /// <param name="TaskName">The task's name.</param>
        /// <param name="Result">The result.</param>
        public bool TryGetTask(string TaskName, out ITask Result)
        {
            return Tasks.TryGetValue(TaskName, out Result);
        }
    }
}

