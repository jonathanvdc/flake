using System;
using System.Collections.Generic;

namespace Flake.Extensibility
{
    /// <summary>
    /// A read-only view of an extension manifest.
    /// </summary>
    public sealed class ExtensionManifestView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Extensibility.ExtensionManifestView"/> class.
        /// </summary>
        /// <param name="Manifest">The manifest to create a view for.</param>
        public ExtensionManifestView(ExtensionManifest Manifest)
        {
            this.manifest = Manifest;
        }

        private ExtensionManifest manifest;

        /// <summary>
        /// Gets the names of all extensions.
        /// </summary>
        /// <value>The extension names.</value>
        public IEnumerable<string> ExtensionNames
        {
            get { return manifest.ExtensionNames; }
        }

        /// <summary>
        /// Gets a mapping of extension names to their paths.
        /// </summary>
        /// <value>The extension name-path map.</value>
        public IReadOnlyDictionary<string, ExtensionPath> ExtensionPaths
        {
            get { return manifest.ExtensionPaths; }
        }

        /// <summary>
        /// Gets a mapping of command types to the extensions that
        /// provide them.
        /// </summary>
        /// <value>The specific command provider paths.</value>
        public IReadOnlyDictionary<string, string> SpecificCommandProviders
        {
            get { return manifest.SpecificCommandProviders; }
        }

        /// <summary>
        /// Gets a mapping of task types to the extensions that
        /// provide them.
        /// </summary>
        /// <value>The specific task provider paths.</value>
        public IReadOnlyDictionary<string, string> SpecificTaskProviders
        {
            get { return manifest.SpecificTaskProviders; }
        }

        /// <summary>
        /// Gets a set of general command provider extension paths.
        /// </summary>
        /// <value>The general command provider paths.</value>
        public IEnumerable<string> GeneralCommandProviders
        {
            get { return manifest.GeneralCommandProviders; }
        }

        /// <summary>
        /// Gets a set of general task provider extension paths.
        /// </summary>
        /// <value>The general task provider paths.</value>
        public IEnumerable<string> GeneralTaskProviders
        {
            get { return manifest.GeneralTaskProviders; }
        }

        /// <summary>
        /// Gets set of extension provider extension paths.
        /// </summary>
        /// <value>The extension provider paths.</value>
        public IEnumerable<string> ExtensionProviders
        {
            get { return manifest.ExtensionProviders; }
        }

        /// <summary>
        /// Tells if this extension manifest contains the 
        /// given extension name.
        /// </summary>
        /// <param name="ExtensionName">The extension name.</param>
        public bool Contains(string ExtensionName)
        {
            return manifest.Contains(ExtensionName);
        }

        /// <summary>
        /// Gets the given path's set of dependencies.
        /// </summary>
        /// <returns>The dependencies.</returns>
        /// <param name="Path">The path.</param>
        public IEnumerable<ExtensionPath> GetDependencies(ExtensionPath Path)
        {
            return manifest.GetDependencies(Path);
        }

        /// <summary>
        /// Gets the given path's set of dependencies, and their dependencies,
        /// and so on.
        /// </summary>
        /// <returns>The recursive dependencies.</returns>
        /// <param name="Path">The path.</param>
        public IEnumerable<ExtensionPath> GetRecursiveDependencies(ExtensionPath Path)
        {
            return manifest.GetRecursiveDependencies(Path);
        }
    }
}

