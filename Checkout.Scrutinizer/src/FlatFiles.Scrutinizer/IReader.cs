﻿using System;
using System.Threading.Tasks;

namespace FlatFiles.Scrutinizer
{
    /// <summary>
    /// Defines the operations that a fixed-length reader must support.
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// Gets the schema being used by the parser to parse record values.
        /// </summary>
        /// <returns>The schema being used by the parser.</returns>
        ISchema GetSchema();

        /// <summary>
        /// Gets the schema being used by the parser to parse record values.
        /// </summary>
        /// <returns>The schema being used by the parser.</returns>
        Task<ISchema> GetSchemaAsync();

        /// <summary>
        /// Reads the next record from the file.
        /// </summary>
        /// <returns>True if the next record was read; otherwise, false if the end of file was reached.</returns>
        bool Read();

        /// <summary>
        /// Reads the next record from the file.
        /// </summary>
        /// <returns>True if the next record was read; otherwise, false if the end of file was reached.</returns>
        ValueTask<bool> ReadAsync();

        /// <summary>
        /// Skips the next record from the file.
        /// </summary>
        /// <returns>True if the next record was skipped; otherwise, false if the end of the file was reached.</returns>
        bool Skip();

        /// <summary>
        /// Skips the next record from the file.
        /// </summary>
        /// <returns>True if the next record was skipped; otherwise, false if the end of the file was reached.</returns>
        ValueTask<bool> SkipAsync();

        /// <summary>
        /// Gets the values of the current record.
        /// </summary>
        /// <returns>The value of the current record.</returns>
        object[] GetValues();
    }
}
