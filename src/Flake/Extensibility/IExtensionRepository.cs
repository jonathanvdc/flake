using System;
using Flame.Compiler;
using System.Collections.Generic;

namespace Flake.Extensibility
{
    /// <summary>
    /// Represents common functionality for extension repositories.
    /// </summary>
    public interface IExtensionRepository
    {
        /// <summary>
        /// Gets a read-only view of the extension manifest.
        /// </summary>
        /// <value>A view of the manifest.</value>
        ExtensionManifestView Manifest { get; }

        /// <summary>
        /// Gets the extension provider for this repository.
        /// </summary>
        /// <value>The extension provider.</value>
        IExtensionProvider ExtensionProvider { get; }

        /// <summary>
        /// Updates the extension manifest.
        /// </summary>
        /// <param name="Update">An action that updates the extension manifest.</param>
        void UpdateManifest(Action<ExtensionManifest> Update);

        /// <summary>
        /// Installs the given extension. The extension is copied from the
        /// source path to the target path.
        /// </summary>
        /// <param name="Value">The extension to install.</param>
        /// <param name="SourcePath">The path to the extension file to install.</param>
        /// <param name="TargetPath">The relative path where the extension path is to be installed.</param>
        ResultOrError<ExtensionPath, LogEntry> Install(
            Extension Value, FileIdentifier SourcePath, ExtensionPath TargetPath);

        /// <summary>
        /// Purges the extension with the given name from the extension 
        /// repository. This method only succeeds if there is no extension
        /// with the given name exists (in which case a 'false' result is returned)
        /// or if no extensions are dependent on this extension.
        /// </summary>
        /// <param name="Name">The name of the extension to purge.</param>
        ResultOrError<bool, LogEntry> Purge(string Name);
    }
}

