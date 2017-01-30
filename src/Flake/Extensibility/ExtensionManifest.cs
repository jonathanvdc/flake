using System;
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
            this.extensionPaths = new Dictionary<string, string>();
            this.specificCommandProviders = new Dictionary<string, string>();
            this.specificTaskProviders = new Dictionary<string, string>();
            this.generalCommandProviders = new HashSet<string>();
            this.generalTaskProviders = new HashSet<string>();
            this.extensionProviders = new HashSet<string>();
        }

        [JsonProperty]
        private Dictionary<string, string> extensionPaths;

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
        /// Gets a mapping of extension names to their paths.
        /// </summary>
        /// <value>The extension name-path map.</value>
        [JsonIgnore]
        public IReadOnlyDictionary<string, string> ExtensionPaths
        {
            get { return extensionPaths; }
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
        /// Registers the providers of the extension with
        /// the given path in the manifest.
        /// </summary>
        /// <param name="ExtensionPath">The extension's path.</param>
        /// <param name="Value">The extension.</param>
        public void Add(Extension Value, string ExtensionPath)
        {
            if (extensionPaths.ContainsKey(Value.Name))
            {
                // If the extension is already known, then we should
                // purge it the manifest first.
                Purge(Value.Name);
            }

            extensionPaths[Value.Name] = ExtensionPath;
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
            return extensionPaths.Remove(ExtensionName)
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

