using System;
using System.IO;
using Flame.Front;

namespace Flake.Extensibility
{
    /// <summary>
    /// A data structure that represents the path of an extension.
    /// </summary>
    public struct ExtensionPath : IEquatable<ExtensionPath>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Extensibility.ExtensionPath"/> struct.
        /// </summary>
        /// <param name="Path">The path itself.</param>
        public ExtensionPath(string Path)
        {
            this.Path = Path;
        }

        /// <summary>
        /// Gets the path to the extension.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the file info of the extension this path points to.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="ExtensionDirectoryPath">The absolute path of the directory that contains the extensions.</param>
        public FileInfo GetFile(string ExtensionDirectoryPath)
        {
            return new FileInfo(
                new PathIdentifier(
                    ExtensionDirectoryPath, Path).AbsolutePath.Path);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Flake.Extensibility.ExtensionPath"/> is equal to the current <see cref="Flake.Extensibility.ExtensionPath"/>.
        /// </summary>
        /// <param name="Other">The <see cref="Flake.Extensibility.ExtensionPath"/> to compare with the current <see cref="Flake.Extensibility.ExtensionPath"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Flake.Extensibility.ExtensionPath"/> is equal to the current
        /// <see cref="Flake.Extensibility.ExtensionPath"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(ExtensionPath Other)
        {
            return Path.Equals(Other.Path);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Flake.Extensibility.ExtensionPath"/>.
        /// </summary>
        /// <param name="Other">The <see cref="System.Object"/> to compare with the current <see cref="Flake.Extensibility.ExtensionPath"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Flake.Extensibility.ExtensionPath"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object Other)
        {
            return Other is ExtensionPath
                ? Equals((ExtensionPath)Other)
                : false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Flake.Extensibility.ExtensionPath"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Flake.Extensibility.ExtensionPath"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Flake.Extensibility.ExtensionPath"/>.</returns>
        public override string ToString()
        {
            return Path;
        }
    }
}

