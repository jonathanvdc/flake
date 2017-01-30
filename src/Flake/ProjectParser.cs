using System;
using System.Collections.Generic;
using System.IO;
using Flame.Compiler;
using Flame.Front;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Flake
{
    /// <summary>
    /// Creates project identifiers, parses projects and retrieves previously
    /// parsed projects.
    /// </summary>
    public sealed class ProjectParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.ProjectParser"/> class.
        /// </summary>
        /// <param name="HandlerProvider">The task handler provider for this project parser.</param>
        public ProjectParser(ITaskHandlerProvider HandlerProvider)
        {
            this.HandlerProvider = HandlerProvider;
            this.projectCache = 
                new Dictionary<ProjectIdentifier, ResultOrError<Project, LogEntry>>();
            this.jsonReader = new JsonSerializer();
        }

        /// <summary>
        /// Gets the task handler provider for this project parser.
        /// </summary>
        /// <value>The task handler provider.</value>
        public ITaskHandlerProvider HandlerProvider { get; private set; }

        private Dictionary<ProjectIdentifier, ResultOrError<Project, LogEntry>> projectCache;
        private JsonSerializer jsonReader;

        /// <summary>
        /// Creates a task identifier from the given project path
        /// and task name. 
        /// </summary>
        /// <returns>The task identifier.</returns>
        /// <param name="ProjectPath">The project's path.</param>
        /// <param name="BasePath">
        /// The path to which the project path is relative.
        /// </param>
        /// <param name="TaskName">The task's name.</param>
        public static ResultOrError<TaskIdentifier, LogEntry> GetIdentifier(
            string ProjectPath, string BasePath, string TaskName)
        {
            return GetIdentifier(ProjectPath, BasePath).MapResult(
                ident => GetIdentifier(ident, TaskName));
        }

        /// <summary>
        /// Creates a project identifier from the given project path.
        /// </summary>
        /// <returns>The project identifier.</returns>
        /// <param name="BasePath">
        /// The path to which the project path is relative.
        /// </param>
        /// <param name="ProjectPath">The project path.</param>
        public static ResultOrError<ProjectIdentifier, LogEntry> GetIdentifier(
            string ProjectPath, string BasePath)
        {
            return GetFileInfo(ProjectPath, BasePath).MapResult(
                info => new ProjectIdentifier(info));
        }

        /// <summary>
        /// Creates a project identifier from the given project path.
        /// </summary>
        /// <returns>The project identifier.</returns>
        /// <param name="ProjectPath">The project path.</param>
        public static ResultOrError<ProjectIdentifier, LogEntry> GetIdentifier(
            string ProjectPath)
        {
            return GetIdentifier(ProjectPath, null);
        }

        private static ResultOrError<FileInfo, LogEntry> GetFileInfo(
            string ProjectPath, string BasePath)
        {
            ResultOrError<FileInfo, LogEntry> result;
            try
            {
                string absPath = BasePath == null 
                    ? ProjectPath
                    : new PathIdentifier(ProjectPath, BasePath).AbsolutePath.Path;
                result = ResultOrError<FileInfo, LogEntry>.CreateResult(
                    new FileInfo(absPath));
            }
            catch (ArgumentNullException)
            {
                result = ResultOrError<FileInfo, LogEntry>.CreateError(
                    new LogEntry(
                        "invalid project path",
                        "project paths cannot be null."));
            }
            catch (ArgumentException)
            {
                result = ResultOrError<FileInfo, LogEntry>.CreateError(
                    new LogEntry(
                        "invalid project path",
                        "path '" + ProjectPath + "' is an ill-formed path."));
            }
            catch (UnauthorizedAccessException)
            {
                result = ResultOrError<FileInfo, LogEntry>.CreateError(
                    new LogEntry(
                        "access denied",
                        "access to the project at '" + ProjectPath + "' was denied."));
            }
            catch (PathTooLongException)
            {
                result = ResultOrError<FileInfo, LogEntry>.CreateError(
                    new LogEntry(
                        "invalid project path",
                        "path '" + ProjectPath + "' exceeds the " +
                        "system-defined maximum path length."));
            }
            catch (NotSupportedException)
            {
                result = ResultOrError<FileInfo, LogEntry>.CreateError(
                    new LogEntry(
                        "invalid project path",
                        "path '" + ProjectPath + "' contains a colon."));
            }
            catch (FormatException ex)
            {
                result = ResultOrError<FileInfo, LogEntry>.CreateError(
                    new LogEntry(
                        "invalid project path",
                        ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Creates a task identifier from the given project identifier
        /// and task name. 
        /// </summary>
        /// <returns>The task identifier.</returns>
        /// <param name="Identifier">The project's identifier.</param>
        /// <param name="TaskName">The task's name.</param>
        public static TaskIdentifier GetIdentifier(
            ProjectIdentifier Identifier, string TaskName)
        {
            return new TaskIdentifier(Identifier, TaskName);
        }

        /// <summary>
        /// Parses the project at the given project path.
        /// </summary>
        /// <param name="ProjectPath">
        /// The project path to parse a project at.
        /// </param>
        /// <param name="BasePath">
        /// The path to which the project path is relative.
        /// </param>
        public ResultOrError<Project, LogEntry> Parse(
            string ProjectPath, string BasePath)
        {
            return GetIdentifier(ProjectPath, BasePath).BindResult(Parse);
        }

        /// <summary>
        /// Parses the project with the given identifier.
        /// </summary>
        /// <param name="Identifier">The identifier for the project to parse.</param>
        public ResultOrError<Project, LogEntry> Parse(
            ProjectIdentifier Identifier)
        {
            ResultOrError<Project, LogEntry> result;
            if (!projectCache.TryGetValue(Identifier, out result))
            {
                result = ParseProject(Identifier);
            }

            return result;
        }

        private ResultOrError<Project, LogEntry> ParseProject(
            ProjectIdentifier Identifier)
        {
            if (!Identifier.File.Exists)
            {
                return ResultOrError<Project, LogEntry>.CreateError(
                    new LogEntry(
                        "missing project file",
                        "project file '" + Identifier.ToString() + "' does not exist."));
            }

            Dictionary<string, JObject> dict;
            using (var reader = Identifier.File.OpenText())
            {
                try
                {
                    dict = jsonReader.Deserialize<Dictionary<string, JObject>>(
                        new JsonTextReader(reader));
                }
                catch (JsonSerializationException)
                {
                    return ResultOrError<Project, LogEntry>.CreateError(
                        new LogEntry(
                            "malformed project file",
                            "project file '" + Identifier.ToString() + "' cannot be read."));
                }
            }

            var tasks = new Dictionary<string, ITask>();
            foreach (var kvPair in dict)
            {
                var taskIdent = new TaskIdentifier(Identifier, kvPair.Key);
                var t = ParseTask(taskIdent, kvPair.Value);
                if (t.IsError)
                    return ResultOrError<Project, LogEntry>.CreateError(t.ErrorOrDefault);

                tasks[kvPair.Key] = t.ResultOrDefault;
            }
            return ResultOrError<Project, LogEntry>.CreateResult(
                new Project(Identifier, tasks));
        }

        private const string TypePropertyName = "type";
        private const string PackagePropertyName = "package";

        private static bool TryGetStringProperty(JObject Obj, string Key, out string Value)
        {
            JToken typeToken;
            if (Obj.TryGetValue(TypePropertyName, out typeToken))
            {
                Value = typeToken.Value<string>();
                return true;
            }
            else
            {
                Value = null;
                return false;
            }
        }

        private static string GetStringPropertyOrNull(JObject Obj, string Key)
        {
            string result;
            TryGetStringProperty(Obj, Key, out result);
            return result;
        }

        private ResultOrError<ITaskHandler, LogEntry> GetTaskHandler(
            TaskIdentifier Identifier, JObject Obj)
        {
            string type;
            if (!TryGetStringProperty(Obj, TypePropertyName, out type))
            {
                return ResultOrError<ITaskHandler, LogEntry>.CreateError(
                    new LogEntry(
                        "invalid task specification",
                        "task '" + Identifier.ToString() + 
                        "' does not have a 'type' property."));
            }

            string package = GetStringPropertyOrNull(Obj, PackagePropertyName);

            var desc = new TaskDescription(type, package);
            return HandlerProvider.GetHandler(desc);
        }

        private ResultOrError<ITask, LogEntry> ParseTask(
            TaskIdentifier Identifier, JObject Obj)
        {
            var handler = GetTaskHandler(Identifier, Obj);
            if (handler.IsError)
            {
                return ResultOrError<ITask, LogEntry>.CreateError(
                    handler.ErrorOrDefault);
            }
            else
            {
                return ResultOrError<ITask, LogEntry>.CreateResult(
                    handler.ResultOrDefault.Parse(Obj, this));
            }
        }
    }
}

