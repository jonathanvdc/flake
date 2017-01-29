using System;
using System.IO;

namespace Flake
{
    /// <summary>
    /// A unique identifier for tasks.
    /// </summary>
    public sealed class TaskIdentifier : IEquatable<TaskIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.TaskIdentifier"/> class.
        /// </summary>
        /// <param name="Project">The project that defines this task.</param>
        /// <param name="TaskName">This task's name.</param>
        public TaskIdentifier(ProjectIdentifier Project, string TaskName)
        {
            this.Project = Project;
            this.TaskName = TaskName;
        }

        /// <summary>
        /// Gets the project's identifier.
        /// </summary>
        /// <value>The project.</value>
        public ProjectIdentifier Project { get; private set; }

        /// <summary>
        /// Gets the task's name.
        /// </summary>
        /// <value>The name of the task.</value>
        public string TaskName { get; private set; }

        /// <param name="First">The first task identifier.</param>
        /// <param name="Second">The second task identifier.</param>
        public static bool operator ==(
            TaskIdentifier First, TaskIdentifier Second)
        {
            return object.Equals(First, Second);
        }

        /// <param name="First">The first task identifier.</param>
        /// <param name="Second">The second task identifier.</param>
        public static bool operator !=(
            TaskIdentifier First, TaskIdentifier Second)
        {
            return !object.Equals(First, Second);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Flake.TaskIdentifier"/> is equal to the current <see cref="Flake.TaskIdentifier"/>.
        /// </summary>
        /// <param name="Other">The <see cref="Flake.TaskIdentifier"/> to compare with the current <see cref="Flake.TaskIdentifier"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Flake.TaskIdentifier"/> is equal to the current
        /// <see cref="Flake.TaskIdentifier"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(TaskIdentifier Other)
        {
            return Project.Equals(Other.Project) 
                && TaskName.Equals(Other.TaskName);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Flake.TaskIdentifier"/>.
        /// </summary>
        /// <param name="Other">The <see cref="System.Object"/> to compare with the current <see cref="Flake.TaskIdentifier"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Flake.TaskIdentifier"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object Other)
        {
            if (Other is TaskIdentifier)
                return Equals((TaskIdentifier)Other);
            else
                return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Flake.TaskIdentifier"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return Project.GetHashCode() ^ TaskName.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Flake.TaskIdentifier"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Flake.TaskIdentifier"/>.</returns>
        public override string ToString()
        {
            return Project.ToString() + ":" + TaskName;
        }
    }
}

