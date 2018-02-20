﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace FlatFiles.Scrutinizer
{
    /// <summary>
    /// Defines the operations that a writer must support.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Gets the schema being used by the builder to create the textual representation.
        /// </summary>
        /// <returns>The schema being used by the builder to create the textual representation.</returns>
        ISchema GetSchema();

        /// <summary>
        /// Writes the textual representation of the given values to the writer.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <returns>The textual representation of the given values.</returns>
        void Write(object[] values);

        /// <summary>
        /// Writes the textual representation of the given values to the writer.
        /// </summary>
        /// <param name="values">The values to write.</param>
        /// <returns>The textual representation of the given values.</returns>
        Task WriteAsync(object[] values);
    }
}
