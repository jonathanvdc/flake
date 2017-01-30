using System;
using System.Collections.Generic;

namespace Flake.Extensibility
{
    /// <summary>
    /// A mutable version of <see cref="Flake.Extensibility.Extension"/>.
    /// </summary>
    public sealed class ExtensionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Extensibility.ExtensionBuilder"/> class.
        /// </summary>
        /// <param name="Name">The extension's name.</param>
        public ExtensionBuilder(string Name)
        {
            this.Name = Name;
            this.SpecificCommandProviders = new Dictionary<string, ICommandProvider>();
            this.SpecificTaskHandlerProviders = new Dictionary<string, ITaskHandlerProvider>();
            this.GeneralCommandProviders = new List<ICommandProvider>();
            this.GeneralTaskHandlerProviders = new List<ITaskHandlerProvider>();
            this.ExtensionProviders = new List<IExtensionProvider>();
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
        public Dictionary<string, ICommandProvider> SpecificCommandProviders { get; private set; }

        /// <summary>
        /// Gets a mapping from specific task types to 
        /// task handler providers in the extension.
        /// </summary>
        /// <value>The specific task handler providers.</value>
        public Dictionary<string, ITaskHandlerProvider> SpecificTaskHandlerProviders { get; private set; }

        /// <summary>
        /// Gets the extension's set of general command providers.
        /// </summary>
        /// <value>The set of general command providers.</value>
        public List<ICommandProvider> GeneralCommandProviders { get; private set; }

        /// <summary>
        /// Gets the extension's set of general task handler providers.
        /// </summary>
        /// <value>The set of general task handler providers.</value>
        public List<ITaskHandlerProvider> GeneralTaskHandlerProviders { get; private set; }

        /// <summary>
        /// Gets the extension's set of extension providers.
        /// </summary>
        /// <value>The extension providers.</value>
        public List<IExtensionProvider> ExtensionProviders { get; private set; }

        /// <summary>
        /// Creates an extension from this extension builder.
        /// </summary>
        /// <returns>The extension.</returns>
        public Extension ToExtension()
        {
            return new Extension(
                Name,
                SpecificCommandProviders,
                SpecificTaskHandlerProviders,
                GeneralCommandProviders,
                GeneralTaskHandlerProviders,
                ExtensionProviders);
        }
    }
}

