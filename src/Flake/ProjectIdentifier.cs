using System;
using System.IO;

namespace Flake
{
    /// <summary>
    /// A unique identifier for a project.
    /// </summary>
    public sealed class ProjectIdentifier : IEquatable<ProjectIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.ProjectIdentifier"/> class.
        /// </summary>
        /// <param name="File">The file that defines this project identifier.</param>
        public ProjectIdentifier(FileInfo File)
        {
            this.File = File;
            this.cachedFullPath = new Lazy<string>(GetFullPath);
        }

        /// <summary>
        /// Gets this project's file.
        /// </summary>
        /// <value>The project's file.</value>
        public FileInfo File { get; private set; }

        /// <summary>
        /// Gets this project's full path.
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath { get { return cachedFullPath.Value; } }

        private Lazy<string> cachedFullPath;
        private string GetFullPath()
        {
            return File.FullName;
        }

        /// <param name="First">The first project identifier.</param>
        /// <param name="Second">The second project identifier.</param>
        public static bool operator ==(
            ProjectIdentifier First, ProjectIdentifier Second)
        {
            return object.Equals(First, Second);
        }

        /// <param name="First">The first project identifier.</param>
        /// <param name="Second">The second project identifier.</param>
        public static bool operator !=(
            ProjectIdentifier First, ProjectIdentifier Second)
        {
            return !object.Equals(First, Second);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Flake.ProjectIdentifier"/> is equal to the current <see cref="Flake.ProjectIdentifier"/>.
        /// </summary>
        /// <param name="Other">The <see cref="Flake.ProjectIdentifier"/> to compare with the current <see cref="Flake.ProjectIdentifier"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Flake.ProjectIdentifier"/> is equal to the current
        /// <see cref="Flake.ProjectIdentifier"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(ProjectIdentifier Other)
        {
            return FullPath.Equals(Other.FullPath, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Flake.ProjectIdentifier"/>.
        /// </summary>
        /// <param name="Other">The <see cref="System.Object"/> to compare with the current <see cref="Flake.ProjectIdentifier"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Flake.ProjectIdentifier"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object Other)
        {
            if (Other is ProjectIdentifier)
                return Equals((ProjectIdentifier)Other);
            else
                return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Flake.ProjectIdentifier"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return FullPath.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Flake.ProjectIdentifier"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Flake.ProjectIdentifier"/>.</returns>
        public override string ToString()
        {
            return FullPath;
        }
    }
}

