using System;
using Flame.Compiler;
using Flake.Commands;

namespace Flake.Driver
{
    /// <summary>
    /// Parses commands from command-line arguments.
    /// </summary>
    public sealed class CommandParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Driver.CommandParser"/> class.
        /// </summary>
        /// <param name="CommandProvider">The command provider.</param>
        /// <param name="TaskHandlerProvider">The task handler provider.</param>
        public CommandParser(
            ICommandProvider CommandProvider,
            ITaskHandlerProvider TaskHandlerProvider)
        {
            this.CommandProvider = CommandProvider;
            this.TaskHandlerProvider = TaskHandlerProvider;
        }

        /// <summary>
        /// Gets the command provider.
        /// </summary>
        /// <value>The command provider.</value>
        public ICommandProvider CommandProvider { get; private set; }

        /// <summary>
        /// Gets the task handler provider.
        /// </summary>
        /// <value>The task handler provider.</value>
        public ITaskHandlerProvider TaskHandlerProvider { get; private set; }

        /// <summary>
        /// Parses a command spec, which may consist of either a filename 
        /// followed by a task name, or a simple command name.
        /// </summary>
        /// <returns>The command spec.</returns>
        /// <param name="FileOrCommand">
        /// The filename, or the command.
        /// </param>
        /// <param name="OptionalTask">
        /// Null, the task, or the first argument to the command.
        /// </param>
        /// <param name="IsTwoWordCommand">
        /// Tells if the parsed command consisted of both a 
        /// filename and a command.
        /// </param>
        public ResultOrError<ICommand, LogEntry> ParseCommandSpec(
            string FileOrCommand, string OptionalTask, out bool IsTwoWordCommand)
        {
            if (FileOrCommand.Equals("--", StringComparison.OrdinalIgnoreCase))
            {
                if (OptionalTask == null)
                {
                    IsTwoWordCommand = false;
                    return ResultOrError<ICommand, LogEntry>.CreateError(
                        new LogEntry(
                            "invalid command specification", 
                            "a pseudo-filename of '--' must be followed " +
                            "by a command name."));
                }
                else
                {
                    IsTwoWordCommand = true;
                    return ParseCommandSpec(OptionalTask);
                }
            }
            else if (OptionalTask == null)
            {
                IsTwoWordCommand = false;
                return ParseCommandSpec(FileOrCommand);
            }
            else
            {
                var identResult = ProjectParser.GetIdentifier(FileOrCommand);
                if (identResult.IsError)
                {
                    IsTwoWordCommand = false;
                    return ResultOrError<ICommand, LogEntry>.CreateError(
                        identResult.ErrorOrDefault);
                }

                var ident = identResult.ResultOrDefault;
                if (!ident.File.Exists)
                {
                    var defaultProj = TryFindDefaultProject();
                    if (defaultProj != null)
                    {
                        IsTwoWordCommand = true;
                        return ResultOrError<ICommand, LogEntry>.CreateResult(
                            ParseCommandSpec(defaultProj, OptionalTask));
                    }
                    else
                    {
                        IsTwoWordCommand = false;
                        return ParseCommandSpec(FileOrCommand);
                    }
                }
                else
                {
                    IsTwoWordCommand = true;
                    return ResultOrError<ICommand, LogEntry>.CreateResult(
                        ParseCommandSpec(ident, OptionalTask));
                }
            }
        }

        /// <summary>
        /// Parses the given simple command name.
        /// </summary>
        /// <returns>The command spec.</returns>
        /// <param name="CommandName">The command name.</param>
        public ResultOrError<ICommand, LogEntry> ParseCommandSpec(string CommandName)
        {
            return CommandProvider.GetCommand(CommandName);
        }

        /// <summary>
        /// Parses the given command spec, which consists of a 
        /// project identifier and a task name.
        /// </summary>
        /// <returns>The command spec.</returns>
        /// <param name="Identifier">The project identifier.</param>
        /// <param name="Command">The task name.</param>
        public ICommand ParseCommandSpec(
            ProjectIdentifier Identifier, string TaskName)
        {
            return new RunTaskCommand(
                TaskHandlerProvider, 
                new TaskIdentifier(Identifier, TaskName));
        }

        /// <summary>
        /// Tries to find the default project identifier.
        /// </summary>
        /// <returns>The default project identifier.</returns>
        public static ProjectIdentifier TryFindDefaultProject()
        {
            var projIdent = ProjectParser.GetIdentifier("flake.json");
            if (!projIdent.IsError && projIdent.ResultOrDefault.File.Exists)
                return projIdent.ResultOrDefault;

            projIdent = ProjectParser.GetIdentifier("Flake.json");
            if (!projIdent.IsError && projIdent.ResultOrDefault.File.Exists)
                return projIdent.ResultOrDefault;
            else
                return null;
        }
    }
}

