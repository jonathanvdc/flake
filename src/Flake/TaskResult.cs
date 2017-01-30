using System;
using System.Collections.Generic;

namespace Flake
{
    /// <summary>
    /// Represents a task's outputs.
    /// </summary>
    public sealed class TaskResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.TaskResult"/> class.
        /// </summary>
        /// <param name="Results">A dictionary that maps output keys to values.</param>
        public TaskResult(
            IReadOnlyDictionary<string, object> Results)
        {
            this.results = Results;
        }

        private IReadOnlyDictionary<string, object> results;

        /// <summary>
        /// The empty task result.
        /// </summary>
        public static readonly TaskResult Empty = new TaskResult(new Dictionary<string, object>()); 

        /// <summary>
        /// Tries to get the value with the given key and value.
        /// </summary>
        /// <returns><c>true</c>, if a value for the given key was found, <c>false</c> otherwise.</returns>
        /// <param name="Key">The value's key.</param>
        /// <param name="Value">The value.</param>
        /// <typeparam name="T">The value's type.</typeparam>
        public bool TryGetValue<T>(string Key, out T Value)
        {
            object val;
            if (results.TryGetValue(Key, out val) && val is T)
            {
                Value = (T)val;
                return true;
            }
            else
            {
                Value = default(T);
                return false;
            }
        }
    }
}

