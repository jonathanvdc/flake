using System;

namespace Flake.Extensibility
{
    /// <summary>
    /// Marks a class, enum or field as a flake import.
    /// </summary>
    /// <remarks>
    /// This attribute must be applied to static fields to import their
    /// values, and to their enclosing types to qualify them for import.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class FlakeImportAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Extensibility.FlakeImportAttribute"/> class.
        /// </summary>
        public FlakeImportAttribute()
        { }
    }
}

