﻿using System;
using System.Collections.Generic;

namespace FlatFiles.Scrutinizer
{
    /// <summary>
    /// Defines the expected format of a fixed-length file record.
    /// </summary>
    public sealed class FixedLengthSchema : Schema
    {
        private readonly List<Window> windows;
        private int totalWidth;

        /// <summary>
        /// Initializes a new instance of a FixedLengthSchema.
        /// </summary>
        public FixedLengthSchema()
        {
            windows = new List<Window>();
        }

        /// <summary>
        /// Adds a column to the schema, using the given definition to define it.
        /// </summary>
        /// <param name="definition">The definition of the column to add.</param>
        /// <param name="window">Describes the column</param>
        /// <returns>The current schema.</returns>
        public FixedLengthSchema AddColumn(IColumnDefinition definition, Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }
            AddColumnBase(definition);
            windows.Add(window);
            totalWidth += window.Width;
            return this;
        }

        /// <summary>
        /// Gets the column widths.
        /// </summary>
        public WindowCollection Windows
        {
            get { return new WindowCollection(windows); }
        }

        /// <summary>
        /// Gets the total width of all columns.
        /// </summary>
        public int TotalWidth
        {
            get { return totalWidth; }
        }

        /// <summary>
        /// Parses the given values assuming that they are in the same order as the column definitions.
        /// </summary>
        /// <param name="values">The values to parse.</param>
        /// <returns>The parsed objects.</returns>
        internal object[] ParseValues(string[] values)
        {
            return ParseValuesBase(values);
        }

        /// <summary>
        /// Formats the given values assuming that they are in the same order as the column definitions.
        /// </summary>
        /// <param name="values">The values to format.</param>
        /// <returns>The formatted values.</returns>
        internal string[] FormatValues(object[] values)
        {
            return FormatValuesBase(values);
        }
    }
}
