using System;
using System.Collections.Generic;

namespace Flake.Extensibility
{
    /// <summary>
    /// Describes what a flake extension entails.
    /// </summary>
    public sealed class Extension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Extensibility.Extension"/> class.
        /// </summary>
        /// <param name="CommandProviders">The set of command providers.</param>
        /// <param name="TaskHandlerProviders">Thet set of task handler providers.</param>
        public Extension(
            IEnumerable<ICommandProvider> CommandProviders,
            IEnumerable<ITaskHandlerProvider> TaskHandlerProviders)
        {
            this.CommandProviders = CommandProviders;
            this.TaskHandlerProviders = TaskHandlerProviders;
        }

        /// <summary>
        /// Gets the extension's set of command providers.
        /// </summary>
        /// <value>The set of command providers.</value>
        public IEnumerable<ICommandProvider> CommandProviders { get; private set; }

        /// <summary>
        /// Gets the extension's set of task handler providers.
        /// </summary>
        /// <value>The set of task handler providers.</value>
        public IEnumerable<ITaskHandlerProvider> TaskHandlerProviders { get; private set; }
    }
}

