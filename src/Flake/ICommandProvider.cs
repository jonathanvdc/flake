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
        /// Tries to find a command for the given type of command.
        /// </summary>
        /// <param name="Name">The command's name</param>
        /// <returns>The command, or an error.</returns>
        ResultOrError<ICommand, LogEntry> GetCommand(string Name);
    }
}

