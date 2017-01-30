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
        /// <param name="Name">The extension's name.</param>
        /// <param name="CommandProviders">The set of command providers.</param>
        /// <param name="TaskHandlerProviders">The set of task handler providers.</param>
        /// <param name="ExtensionProviders">The set of extension providers.</param>
        public Extension(
            string Name,
            IEnumerable<ICommandProvider> CommandProviders,
            IEnumerable<ITaskHandlerProvider> TaskHandlerProviders,
            IEnumerable<IExtensionProvider> ExtensionProviders)
        {
            this.Name = Name;
            this.CommandProviders = CommandProviders;
            this.TaskHandlerProviders = TaskHandlerProviders;
            this.ExtensionProviders = ExtensionProviders;
        }

        /// <summary>
        /// Gets the extension's name.
        /// </summary>
        /// <value>The extension's name.</value>
        public string Name { get; private set; }

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

        /// <summary>
        /// Gets the extension's set of extension providers.
        /// </summary>
        /// <value>The extension providers.</value>
        public IEnumerable<IExtensionProvider> ExtensionProviders { get; private set; }
    }
}

