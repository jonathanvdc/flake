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
        /// <param name="SpecificCommandProviders">
        /// A mapping from specific command names to command providers.
        /// </param>        
        /// <param name="SpecificTaskHandlerProviders">
        /// A mapping from specific task types to task handler providers.
        /// </param>
        /// <param name="GeneralCommandProviders">The set of command providers.</param>
        /// <param name="GeneralTaskHandlerProviders">The set of task handler providers.</param>
        /// <param name="ExtensionProviders">The set of extension providers.</param>
        public Extension(
            string Name,
            IReadOnlyDictionary<string, ICommandProvider> SpecificCommandProviders,
            IReadOnlyDictionary<string, ITaskHandlerProvider> SpecificTaskHandlerProviders,
            IEnumerable<ICommandProvider> GeneralCommandProviders,
            IEnumerable<ITaskHandlerProvider> GeneralTaskHandlerProviders,
            IEnumerable<IExtensionProvider> ExtensionProviders)
        {
            this.Name = Name;
            this.SpecificCommandProviders = SpecificCommandProviders;
            this.SpecificTaskHandlerProviders = SpecificTaskHandlerProviders;
            this.GeneralCommandProviders = GeneralCommandProviders;
            this.GeneralTaskHandlerProviders = GeneralTaskHandlerProviders;
            this.ExtensionProviders = ExtensionProviders;
        }

        /// <summary>
        /// Gets the extension's name.
        /// </summary>
        /// <value>The extension's name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a mapping from specific command names to 
        /// command providers in the extension.
        /// </summary>
        /// <value>The specific command providers.</value>
        public IReadOnlyDictionary<string, ICommandProvider> SpecificCommandProviders { get; private set; }

        /// <summary>
        /// Gets a mapping from specific task types to 
        /// task handler providers in the extension.
        /// </summary>
        /// <value>The specific task handler providers.</value>
        public IReadOnlyDictionary<string, ITaskHandlerProvider> SpecificTaskHandlerProviders { get; private set; }

        /// <summary>
        /// Gets the extension's set of general command providers.
        /// </summary>
        /// <value>The set of general command providers.</value>
        public IEnumerable<ICommandProvider> GeneralCommandProviders { get; private set; }

        /// <summary>
        /// Gets the extension's set of general task handler providers.
        /// </summary>
        /// <value>The set of general task handler providers.</value>
        public IEnumerable<ITaskHandlerProvider> GeneralTaskHandlerProviders { get; private set; }

        /// <summary>
        /// Gets the extension's set of extension providers.
        /// </summary>
        /// <value>The extension providers.</value>
        public IEnumerable<IExtensionProvider> ExtensionProviders { get; private set; }
    }
}

