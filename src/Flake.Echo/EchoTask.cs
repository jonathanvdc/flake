using System;
using System.Collections.Generic;
using Flake.Extensibility;
using Flame.Compiler;
using Newtonsoft.Json;

namespace Flake.Echo
{
    /// <summary>
    /// A task that prints a line of text.
    /// </summary>
    [FlakeImport]
    public sealed class EchoTask : ITask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Echo.EchoTask"/> class.
        /// </summary>
        public EchoTask()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Echo.EchoTask"/> class.
        /// </summary>
        /// <param name="Text">The text to print.</param>
        public EchoTask(string Text)
        {
            this.Text = Text;
        }

        /// <summary>
        /// Gets the line of text to print.
        /// </summary>
        /// <value>The text to print.</value>
        public string Text { get; private set; }

        /// <summary>
        /// The handler for echo tasks.
        /// </summary>
        [FlakeImport]
        public static readonly ITaskHandler Handler = 
            new SerializedTaskHandler<EchoTask>("echo");

        /// <inheritdoc/>
        [JsonIgnore]
        public IReadOnlyList<TaskIdentifier> Dependencies 
        { 
            get { return new TaskIdentifier[0]; } 
        }

        /// <inheritdoc/>
        public ResultOrError<TaskResult, LogEntry> Run(
            TaskState State, ICompilerLog Log)
        {
            EchoCommand.Instance.Run(new string[] { Text }, Log);
            return ResultOrError<TaskResult, LogEntry>.CreateResult(
                TaskResult.Empty);
        }
    }
}

