﻿using System;
using Flame.Compiler;

namespace Flake
{
    /// <summary>
    /// Defines common functionality for objects that create
    /// task handlers based for specific task types. 
    /// </summary>
    public interface ITaskHandlerProvider
    {
        /// <summary>
        /// Tries to find a handler for the given type of task.
        /// </summary>
        /// <param name="Description">The task type's description</param>
        /// <returns>The task handler, or an error.</returns>
        ResultOrError<ITaskHandler, LogEntry> GetHandler(TaskDescription Description);
    }
}
