﻿using System;
using Newtonsoft.Json.Linq;

namespace Flake
{
    /// <summary>
    /// Defines common functionality for objects that 
    /// parse flake tasks. 
    /// </summary>
    public interface ITaskHandler
    {
        /// <summary>
        /// Gets this task handler's task type.
        /// </summary>
        /// <value>The task type.</value>
        string TaskType { get; }

        /// <summary>
        /// Parses the given JSON object as a task.
        /// </summary>
        /// <returns>A parsed task.</returns>
        /// <param name="Object">The object to parse.</param>
        /// <param name="Parser">The project parser.</param>
        ITask Parse(JObject Object, ProjectParser Parser);
    }
}

