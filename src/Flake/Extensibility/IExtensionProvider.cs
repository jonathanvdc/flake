using System;
using Flame.Compiler;

namespace Flake.Extensibility
{
    /// <summary>
    /// Defines common functionality for extension providers.
    /// </summary>
    public interface IExtensionProvider
    {
        /// <summary>
        /// Retrieves the extension with the given identifier.
        /// </summary>
        /// <returns>The extension.</returns>
        /// <param name="Identifier">The extension's identifier.</param>
        /// <param name="Log">The log to which diagnostics may be sent.</param>
        ResultOrError<Extension, LogEntry> GetExtension(string Identifier, ICompilerLog Log);
    }
}

