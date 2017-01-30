using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flake
{
    /// <summary>
    /// A task handler that deserializes JSON objects to tasks.
    /// </summary>
    public sealed class SerializedTaskHandler<T> : ITaskHandler
        where T : ITask, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.SerializedTaskHandler&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="TaskType">The type of tasks that is handled.</param>
        public SerializedTaskHandler(string TaskType) 
        {
            this.TaskType = TaskType;
        }

        /// <inheritdoc/>
        public string TaskType { get; private set; }

        /// <inheritdoc/>
        public ITask Parse(JObject Object, ProjectParser Parser)
        {
            return Object.ToObject<T>();
        }
    }
}

