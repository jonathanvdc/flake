using System;

namespace Flake
{
    /// <summary>
    /// Describes a task's type, and where it can be found.
    /// </summary>
    public sealed class TaskDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.TaskDescription"/> class.
        /// </summary>
        /// <param name="Type">The task's type.</param>
        public TaskDescription(string Type)
            : this(Type, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.TaskDescription"/> class.
        /// </summary>
        /// <param name="Type">The task's type.</param>
        /// <param name="Package">The name of the package that defines the task's type.</param>
        public TaskDescription(
            string Type, string Package)
        {
            this.Type = Type;
            this.Package = Package;
        }

        /// <summary>
        /// Gets the task's type.
        /// </summary>
        /// <value>The task's type.</value>
        public string Type { get; private set; }

        /// <summary>
        /// Gets the name of the package that defines the task's type.
        /// </summary>
        /// <value>The name of the package that defines the task's type.</value>
        public string Package { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this task description includes a package.
        /// </summary>
        /// <value><c>true</c> if this task description includes a package; otherwise, <c>false</c>.</value>
        public bool HasPackage { get { return Package != null; } }

        public override string ToString()
        {
            if (HasPackage)
            {
                return Type + " (from " + Package + ")";
            }
            else
            {
                return Type;
            }
        }
    }
}

