using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Flake.Extensibility;
using Flame.Compiler;
using Flake.Providers;

namespace Flake.Clr
{
    /// <summary>
    /// An extension provider for CLR libraries.
    /// </summary>
    public sealed class ClrExtensionProvider : IExtensionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Clr.ClrExtensionProvider"/> class.
        /// </summary>
        /// <param name="Manifest">The extension manifest.</param>
        public ClrExtensionProvider(ExtensionManifest Manifest)
        {
            this.Manifest = Manifest;
        }

        /// <summary>
        /// Gets the extension manifest this extension provider
        /// consults.
        /// </summary>
        /// <value>The manifest.</value>
        public ExtensionManifest Manifest { get; private set; }

        /// <summary>
        /// Retrieves the extension with the given identifier.
        /// </summary>
        /// <returns>The extension.</returns>
        /// <param name="Identifier">The extension's identifier.</param>
        /// <param name="Log">The log to which diagnostics may be sent.</param>
        public ResultOrError<Extension, LogEntry> GetExtension(string Identifier, ICompilerLog Log)
        {
            string path;
            if (!Manifest.ExtensionPaths.TryGetValue(Identifier, out path))
            {
                return ResultOrError<Extension, LogEntry>.CreateError(
                    new LogEntry(
                        "unknown extension",
                        "extension '" + Identifier + "' does not " +
                        "appear in the local manifest."));
            }

            var asm = Assembly.LoadFrom(path);
            var ext = new ExtensionBuilder(Identifier);
            ImportTypes(asm.ExportedTypes, ext);
            return ResultOrError<Extension, LogEntry>.CreateResult(
                ext.ToExtension());
        }

        private static void ImportTypes(
            IEnumerable<Type> Types, ExtensionBuilder Extension)
        {
            foreach (var ty in Types)
            {
                ImportType(ty, Extension);
            }
        }

        private static void ImportType(
            Type Type, ExtensionBuilder Extension)
        {
            if (IsFlakeImport(Type))
            {
                ImportTypes(Type.GetNestedTypes(), Extension);
                foreach (var field in Type.GetFields())
                {
                    ImportField(field, Extension);
                }
            }
        }

        private static void ImportField(
            FieldInfo Field, ExtensionBuilder Extension)
        {
            if (!IsFlakeImport(Field) || !Field.IsStatic)
                return;

            var fieldTy = Field.FieldType;
            if (typeof(ICommandProvider).IsAssignableFrom(fieldTy))
            {
                Extension.GeneralCommandProviders.Add(
                    (ICommandProvider)Field.GetValue(null));
            }
            if (typeof(ITaskHandlerProvider).IsAssignableFrom(fieldTy))
            {
                Extension.GeneralTaskHandlerProviders.Add(
                    (ITaskHandlerProvider)Field.GetValue(null));
            }
            if (typeof(IExtensionProvider).IsAssignableFrom(fieldTy))
            {
                Extension.ExtensionProviders.Add(
                    (IExtensionProvider)Field.GetValue(null));
            }
            if (typeof(ICommand).IsAssignableFrom(fieldTy))
            {
                var cmd = (ICommand)Field.GetValue(null);
                Extension.SpecificCommandProviders[cmd.Name] =
                    new SingleCommandProvider(cmd);
            }
            if (typeof(ITaskHandler).IsAssignableFrom(fieldTy))
            {
                var handler = (ITaskHandler)Field.GetValue(null);
                Extension.SpecificTaskHandlerProviders[handler.TaskType] =
                    new SingleTaskHandlerProvider(handler);
            }
        }

        private static bool IsFlakeImport(
            Type Type)
        {
            return Type.GetCustomAttributes(typeof(FlakeImportAttribute)).Any();
        }

        private static bool IsFlakeImport(
            FieldInfo Field)
        {
            return Field.GetCustomAttributes(typeof(FlakeImportAttribute)).Any();
        }
    }
}

