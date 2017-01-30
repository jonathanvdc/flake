using System;
using System.Collections.Generic;
using System.Linq;
using Flame.Compiler;

namespace Flake.Commands
{
    /// <summary>
    /// A command that parses a project, selects a task, and runs it
    /// along with all of its dependencies.
    /// </summary>
    public sealed class RunTaskCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Commands.RunTaskCommand"/> class.
        /// </summary>
        /// <param name="TaskHandlerProvider">The task handler provider.</param>
        public RunTaskCommand(ITaskHandlerProvider TaskHandlerProvider)
            : this(TaskHandlerProvider, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Commands.RunTaskCommand"/> class.
        /// </summary>
        /// <param name="TaskHandlerProvider">The task handler provider.</param>
        /// <param name="TaskSpec">The (optional) task specification.</param>
        public RunTaskCommand(
            ITaskHandlerProvider TaskHandlerProvider,
            TaskIdentifier TaskSpec)
        {
            this.TaskHandlerProvider = TaskHandlerProvider;
            this.TaskSpec = TaskSpec;
        }

        /// <summary>
        /// Gets the task handler provider.
        /// </summary>
        /// <value>The task handler provider.</value>
        public ITaskHandlerProvider TaskHandlerProvider { get; private set; }

        /// <summary>
        /// Gets the task specification, if one has been pre-specified.
        /// </summary>
        /// <value>The task specification, if one has been pre-specified.</value>
        public TaskIdentifier TaskSpec { get; private set; }

        /// <inheritdoc/>
        public string Name { get { return "run"; } }

        /// <inheritdoc/>
        public void Run(IReadOnlyList<string> Arguments, ICompilerLog Log)
        {
            TaskIdentifier spec;
            if (TaskSpec == null)
            {
                if (Arguments.Count <= 2)
                {
                    Log.LogError(
                        new LogEntry(
                            "syntax error",
                            "the 'run' command requires at least two arguments: " +
                            "the filename of the project and the name of the task."));
                    return;
                }

                var fileName = ProjectParser.GetIdentifier(Arguments[0]);
                if (fileName.IsError)
                {
                    Log.LogError(fileName.ErrorOrDefault);
                    return;
                }

                string taskName = Arguments[1];
                spec = new TaskIdentifier(fileName.ResultOrDefault, taskName);
            }
            else
            {
                spec = TaskSpec;
            }

            var parser = new ProjectParser(TaskHandlerProvider);
            var dependencyGraph = new Graph<TaskIdentifier>();
            var tasks = new Dictionary<TaskIdentifier, ITask>();

            // Parse all tasks and construct the dependency graph.
            if (!ParseTaskAndDependencies(spec, parser, dependencyGraph, tasks, Log))
                return;

            // Run all tasks.
            RunAllTasks(dependencyGraph, tasks, Log);
        }

        private static bool ParseTaskAndDependencies(
            TaskIdentifier TaskSpec, ProjectParser Parser, 
            Graph<TaskIdentifier> Dependencies, 
            Dictionary<TaskIdentifier, ITask> Tasks,
            ICompilerLog Log)
        {
            if (Tasks.ContainsKey(TaskSpec))
                return true;

            var proj = Parser.Parse(TaskSpec.Project);
            if (proj.IsError)
            {
                Log.LogError(proj.ErrorOrDefault);
                return false;
            }

            ITask task;
            if (!proj.ResultOrDefault.TryGetTask(TaskSpec.TaskName, out task))
            {
                Log.LogError(
                    new LogEntry(
                        "missing task",
                        "project '" + proj.ResultOrDefault.Identifier.ToString() + 
                        "' does not define a task called '" + TaskSpec.TaskName + "'."));
                return false;
            }

            Tasks[TaskSpec] = task;
            Dependencies.AddVertex(TaskSpec);
            foreach (var dependencySpec in task.Dependencies)
            {
                Dependencies.AddEdge(TaskSpec, dependencySpec);
                if (!ParseTaskAndDependencies(
                    dependencySpec, Parser, Dependencies, Tasks, Log))
                {
                    return false;
                }
            }

            return true;
        }

        private static void RunAllTasks(
            Graph<TaskIdentifier> DependencyGraph,
            Dictionary<TaskIdentifier, ITask> Tasks,
            ICompilerLog Log)
        {
            var state = new TaskStateBuilder();
            while (Tasks.Count > 0)
            {
                TaskIdentifier tSpec;
                if (!TryPopDependencySatisfiedTask(
                    DependencyGraph, out tSpec))
                {
                    Log.LogError(new LogEntry(
                        "cyclic dependency",
                        "the dependency graph contains " +
                        "at least one cycle."));
                    return;
                }

                var tResult = Tasks[tSpec].Run(new TaskState(state), Log);
                if (tResult.IsError)
                {
                    Log.LogError(tResult.ErrorOrDefault);
                }
                else
                {
                    state.Complete(tSpec, tResult.ResultOrDefault);
                }
            }
        }

        /// <summary>
        /// Tries to pop a task from the given dependency graph that
        /// has all of its dependencies satisfied.
        /// </summary>
        private static bool TryPopDependencySatisfiedTask(
            Graph<TaskIdentifier> DependencyGraph,
            out TaskIdentifier TaskSpec)
        {
            foreach (var v in DependencyGraph.Vertices)
            {
                if (!DependencyGraph.GetOutgoingEdges(v).Any())
                {
                    DependencyGraph.RemoveVertex(v);
                    TaskSpec = v;
                    return true;
                }
            }
            TaskSpec = default(TaskIdentifier);
            return false;
        }
    }
}

