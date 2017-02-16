using System;

namespace Flake.Extensibility
{
    /// <summary>
    /// Lists possible extension management schemes.
    /// </summary>
    public enum ExtensionManagementScheme
    {
        /// <summary>
        /// The extension is managed automatically: it is merely a means to
        /// an end, i.e., a dependency of another extension.
        /// </summary>
        Automatic,
        /// <summary>
        /// The extension is managed manually: it has been installed by the user
        /// and should not be removed because no other extension depends on it.
        /// </summary>
        Manual
    }
}

