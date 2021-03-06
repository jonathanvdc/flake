﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Flake.Extensibility
{
    /// <summary>
    /// A data structure that maps type names to extension paths.
    /// </summary>
    public sealed class ExtensionManifest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Extensibility.ExtensionManifest"/> class.
        /// </summary>
        public ExtensionManifest()
        {
            this.extensionPaths = new Dictionary<string, ExtensionPath>();
            this.extensionManagement = new Dictionary<string, ExtensionManagementScheme>();
            this.extensionDependencies = new Graph<ExtensionPath>();
            this.specificCommandProviders = new Dictionary<string, string>();
            this.specificTaskProviders = new Dictionary<string, string>();
            this.generalCommandProviders = new HashSet<string>();
            this.generalTaskProviders = new HashSet<string>();
            this.extensionProviders = new HashSet<string>();
        }

        [JsonProperty]
        private Dictionary<string, ExtensionPath> extensionPaths;

        [JsonProperty]
        private Dictionary<string, ExtensionManagementScheme> extensionManagement;

        [JsonProperty]
        private Graph<ExtensionPath> extensionDependencies;

        [JsonProperty]
        private Dictionary<string, string> specificCommandProviders;

        [JsonProperty]
        private Dictionary<string, string> specificTaskProviders;

        [JsonProperty]
        private HashSet<string> generalCommandProviders;

        [JsonProperty]
        private HashSet<string> generalTaskProviders;

        [JsonProperty]
        private HashSet<string> extensionProviders;

        /// <summary>
        /// Gets the names of all extensions.
        /// </summary>
        /// <value>The extension names.</value>
        [JsonIgnore]
        public IEnumerable<string> ExtensionNames
        {
            get { return extensionPaths.Keys; }
        }

        /// <summary>
        /// Gets a mapping of extension names to their paths.
        /// </summary>
        /// <value>The extension name-path map.</value>
        [JsonIgnore]
        public IReadOnlyDictionary<string, ExtensionPath> ExtensionPaths
        {
            get { return extensionPaths; }
        }

        /// <summary>
        /// Gets the dependency graph for all extensions and their dependencies.
        /// </summary>
        /// <value>The extension dependencies.</value>
        [JsonIgnore]
        public Graph<ExtensionPath> ExtensionDependencies
        {
            get { return extensionDependencies; }
        }

        /// <summary>
        /// Gets a mapping of command types to the extensions that
        /// provide them.
        /// </summary>
        /// <value>The specific command provider paths.</value>
        [JsonIgnore]
        public IReadOnlyDictionary<string, string> SpecificCommandProviders
        {
            get { return specificCommandProviders; }
        }

        /// <summary>
        /// Gets a mapping of task types to the extensions that
        /// provide them.
        /// </summary>
        /// <value>The specific task provider paths.</value>
        [JsonIgnore]
        public IReadOnlyDictionary<string, string> SpecificTaskProviders
        {
            get { return specificTaskProviders; }
        }

        /// <summary>
        /// Gets a set of general command provider extension paths.
        /// </summary>
        /// <value>The general command provider paths.</value>
        [JsonIgnore]
        public IEnumerable<string> GeneralCommandProviders
        {
            get { return generalCommandProviders; }
        }

        /// <summary>
        /// Gets a set of general task provider extension paths.
        /// </summary>
        /// <value>The general task provider paths.</value>
        [JsonIgnore]
        public IEnumerable<string> GeneralTaskProviders
        {
            get { return generalTaskProviders; }
        }

        /// <summary>
        /// Gets set of extension provider extension paths.
        /// </summary>
        /// <value>The extension provider paths.</value>
        [JsonIgnore]
        public IEnumerable<string> ExtensionProviders
        {
            get { return extensionProviders; }
        }

        /// <summary>
        /// Tells if this extension manifest contains the 
        /// given extension name.
        /// </summary>
        /// <param name="ExtensionName">The extension name.</param>
        public bool Contains(string ExtensionName)
        {
            return extensionPaths.ContainsKey(ExtensionName);
        }

        /// <summary>
        /// Gets the given path's set of dependencies.
        /// </summary>
        /// <returns>The dependencies.</returns>
        /// <param name="Path">The path.</param>
        public IEnumerable<ExtensionPath> GetDependencies(ExtensionPath Path)
        {
            if (extensionDependencies.ContainsVertex(Path))
                return extensionDependencies.GetOutgoingEdges(Path);
            else
                return Enumerable.Empty<ExtensionPath>();
        }

        /// <summary>
        /// Gets the given path's set of dependencies, and their dependencies,
        /// and so on.
        /// </summary>
        /// <returns>The recursive dependencies.</returns>
        /// <param name="Path">The path.</param>
        public IEnumerable<ExtensionPath> GetRecursiveDependencies(ExtensionPath Path)
        {
            if (extensionDependencies.ContainsVertex(Path))
                return extensionDependencies.GetReachableVertices(Path);
            else
                return Enumerable.Empty<ExtensionPath>();
        }

        /// <summary>
        /// Registers the providers of the extension with
        /// the given path in the manifest.
        /// </summary>
        /// <param name="Path">The extension's path.</param>
        /// <param name="Value">The extension.</param>
        public void Add(Extension Value, ExtensionPath Path)
        {
            if (extensionPaths.ContainsKey(Value.Name))
            {
                // If the extension is already known, then we should
                // purge it the manifest first.
                Purge(Value.Name);
            }

            extensionPaths[Value.Name] = Path;
            extensionManagement[Value.Name] = ExtensionManagementScheme.Automatic;
            RegisterProviders(
                Value.Name, Value.SpecificCommandProviders.Keys,
                specificCommandProviders);
            RegisterProviders(
                Value.Name, Value.SpecificTaskHandlerProviders.Keys,
                specificTaskProviders);
            
            if (Value.GeneralCommandProviders.Any())
                generalCommandProviders.Add(Value.Name);

            if (Value.GeneralTaskHandlerProviders.Any())
                generalTaskProviders.Add(Value.Name);

            if (Value.ExtensionProviders.Any())
                extensionProviders.Add(Value.Name);
        }

        /// <summary>
        /// Changes the extension management scheme of the extension with the given name.
        /// </summary>
        /// <param name="ExtensionName">The name of the extension whose management scheme is to be changed.</param>
        /// <param name="Scheme">The management scheme to change.</param>
        public void ChangeExtensionManagementScheme(
            string ExtensionName, ExtensionManagementScheme Scheme)
        {
            extensionManagement[ExtensionName] = Scheme;
        }

        /// <summary>
        /// Makes the second path dependent on the first.
        /// </summary>
        /// <param name="DependencyPath">
        /// The path to the dependency.
        /// </param>
        /// <param name="DependentPath">
        /// The path to the file which depends on the dependency.
        /// </param>
        public void AddDependency(
            ExtensionPath DependencyPath,
            ExtensionPath DependentPath)
        {
            extensionDependencies.AddEdge(DependentPath, DependencyPath);
        }

        private static void RegisterProviders(
            string ExtensionName,
            IEnumerable<string> ExtensionSpecificProviders,
            Dictionary<string, string> SpecificProviderMap)
        {
            foreach (var item in ExtensionSpecificProviders)
            {
                SpecificProviderMap[item] = ExtensionName;
            }
        }

        /// <summary>
        /// Purges the extension with the given name from 
        /// the manifest.
        /// </summary>
        /// <param name="ExtensionName">The extension's name</param>
        /// <returns><c>true</c> if the manifest has changed; otherwise, <c>false</c>.</returns>
        public bool Purge(string ExtensionName)
        {
            ExtensionPath path;
            bool foundPath = extensionPaths.TryGetValue(ExtensionName, out path);
            if (foundPath)
                extensionDependencies.RemoveVertex(path);

            return foundPath
                | extensionPaths.Remove(ExtensionName)
                | extensionManagement.Remove(ExtensionName)
                | PurgeValueFrom(specificCommandProviders, ExtensionName)
                | PurgeValueFrom(specificTaskProviders, ExtensionName)
                | generalCommandProviders.Remove(ExtensionName)
                | generalTaskProviders.Remove(ExtensionName)
                | extensionProviders.Remove(ExtensionName);
        }

        private static bool PurgeValueFrom(
            Dictionary<string, string> Dictionary, string Value)
        {
            var keys = Dictionary.Keys
                .Where(k => 
                    Dictionary[k] == Value)
                .ToArray();

            if (keys.Length > 0)
            {
                foreach (var k in keys)
                    Dictionary.Remove(k);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

