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
        private SerializedTaskHandler() 
        { }

        /// <summary>
        /// The serialized task handler's instance for this type.
        /// </summary>
        public static readonly SerializedTaskHandler<T> Instance = 
            new SerializedTaskHandler<T>();

        /// <inheritdoc/>
        public ITask Parse(JObject Object)
        {
            return Object.ToObject<T>();
        }
    }
}

