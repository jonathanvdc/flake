using System;
using System.Collections.Generic;
using Flame.Compiler;
using Pixie;

namespace Flake.Extensibility
{
    /// <summary>
    /// A class that manages extensions.
    /// </summary>
    public sealed class ExtensionManager : 
        ICommandProvider, ITaskHandlerProvider, 
        IExtensionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Extensibility.ExtensionManager"/> class.
        /// </summary>
        public ExtensionManager()
        {
            this.cachedExtensions = new Dictionary<string, ResultOrError<Extension, LogEntry>>();
            this.loadedExtensionNames = new HashSet<string>();
            this.specificCommandProviders = new Dictionary<string, ICommandProvider>();
            this.specificTaskProviders = new Dictionary<string, ITaskHandlerProvider>();
            this.commandProviders = new List<ICommandProvider>();
            this.taskHandlerProviders = new List<ITaskHandlerProvider>();
            this.extensionProviders = new List<IExtensionProvider>();
        }

        private Dictionary<string, ResultOrError<Extension, LogEntry>> cachedExtensions;
        private HashSet<string> loadedExtensionNames;
        private Dictionary<string, ICommandProvider> specificCommandProviders;
        private Dictionary<string, ITaskHandlerProvider> specificTaskProviders;
        private List<ICommandProvider> commandProviders;
        private List<ITaskHandlerProvider> taskHandlerProviders;
        private List<IExtensionProvider> extensionProviders;

        /// <summary>
        /// Adds the given extension to this extension manager,
        /// if no extension with the given identifier has been
        /// loaded yet.
        /// </summary>
        /// <returns><c>true</c>, if the extension was added, <c>false</c> otherwise.</returns>
        /// <param name="Value">The extension to load.</param>
        public bool LoadExtension(Extension Value)
        {
            if (loadedExtensionNames.Add(Value.Name))
            {
                foreach (var kvPair in Value.SpecificCommandProviders)
                {
                    specificCommandProviders[kvPair.Key] = kvPair.Value;
                }
                foreach (var kvPair in Value.SpecificTaskHandlerProviders)
                {
                    specificTaskProviders[kvPair.Key] = kvPair.Value;
                }
                taskHandlerProviders.AddRange(Value.GeneralTaskHandlerProviders);
                commandProviders.AddRange(Value.GeneralCommandProviders);
                taskHandlerProviders.AddRange(Value.GeneralTaskHandlerProviders);
                extensionProviders.AddRange(Value.ExtensionProviders);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public ResultOrError<ICommand, LogEntry> GetCommand(
            string Name, ICompilerLog Log)
        {
            ICommandProvider specificProvider;
            if (specificCommandProviders.TryGetValue(Name, out specificProvider))
            {
                return specificProvider.GetCommand(Name, Log);
            }

            var errorMessages = new List<MarkupNode>();
            foreach (var item in commandProviders)
            {
                var result = item.GetCommand(Name, Log);
                if (result.IsError)
                {
                    errorMessages.Add(result.ErrorOrDefault.Contents);
                }
                else
                {
                    return result;
                }
            }
            return ResultOrError<ICommand, LogEntry>.CreateError(
                new LogEntry(
                    "unknown command",
                    ListExtensions.Instance.CreateList(
                        "cannot find a command named '" + Name + "'.", 
                        errorMessages)));
        }

        /// <inheritdoc/>
        public ResultOrError<ITaskHandler, LogEntry> GetHandler(
            TaskDescription Description, ICompilerLog Log)
        {
            ITaskHandlerProvider specificProvider;
            if (specificTaskProviders.TryGetValue(Description.Type, out specificProvider))
            {
                return specificProvider.GetHandler(Description, Log);
            }

            var errorMessages = new List<MarkupNode>();
            foreach (var item in taskHandlerProviders)
            {
                var result = item.GetHandler(Description, Log);
                if (result.IsError)
                {
                    errorMessages.Add(result.ErrorOrDefault.Contents);
                }
                else
                {
                    return result;
                }
            }
            return ResultOrError<ITaskHandler, LogEntry>.CreateError(
                new LogEntry(
                    "unknown task type",
                    ListExtensions.Instance.CreateList(
                        "cannot find a task type named '" + 
                        Description.ToString() + "'.", 
                        errorMessages)));
        }

        /// <inheritdoc/>
        public ResultOrError<Extension, LogEntry> GetExtension(
            string Identifier, ICompilerLog Log)
        {
            ResultOrError<Extension, LogEntry> result;
            if (!cachedExtensions.TryGetValue(Identifier, out result))
            {
                var errorMessages = new List<MarkupNode>();
                foreach (var item in extensionProviders)
                {
                    result = item.GetExtension(Identifier, Log);
                    if (result.IsError)
                    {
                        errorMessages.Add(result.ErrorOrDefault.Contents);
                    }
                    else
                    {
                        cachedExtensions[Identifier] = result;
                        return result;
                    }
                }

                result = ResultOrError<Extension, LogEntry>.CreateError(
                    new LogEntry(
                        "unknown extension",
                        ListExtensions.Instance.CreateList(
                            "cannot find an extension named '" + 
                            Identifier + "'.", 
                            errorMessages)));
                cachedExtensions[Identifier] = result;
            }
            return result;
        }
    }
}

