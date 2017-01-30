using System;
using Flame.Compiler;

namespace Flake
{
    /// <summary>
    /// Defines common functionality for objects that create
    /// commands based on their names. 
    /// </summary>
    public interface ICommandProvider
    {
        /// <summary>
        /// Tries to find a command with the given type.
        /// </summary>
        /// <param name="Name">The command's name</param>
        /// <param name="Log">The log to which diagnostics may be sent.</param>
        /// <returns>The command, or an error.</returns>
        ResultOrError<ICommand, LogEntry> GetCommand(
            string Name, ICompilerLog Log);
    }
}

