using System;
using System.Collections.Generic;
using Flame.Compiler;

namespace Flake
{
    /// <summary>
    /// Defines commands: sub-programs that can be run independently,
    /// and parse their own input.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the command's name.
        /// </summary>
        /// <value>The command's name.</value>
        string Name { get; }

        /// <summary>
        /// Runs this command with the given arguments and log.
        /// </summary>
        /// <param name="Arguments">The command's arguments.</param>
        /// <param name="Log">The log to which diagnostics can be sent.</param>
        void Run(IReadOnlyList<string> Arguments, ICompilerLog Log);
    }
}

