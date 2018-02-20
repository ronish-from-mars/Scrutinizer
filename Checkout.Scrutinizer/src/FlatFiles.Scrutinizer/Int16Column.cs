﻿using System;
using System.Globalization;
using System.Text;

namespace FlatFiles.Scrutinizer
{
    /// <summary>
    /// Represents a column containing 16-bit integers.
    /// </summary>
    public class Int16Column : ColumnDefinition
    {
        /// <summary>
        /// Initializes a new instance of an Int16Column.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        public Int16Column(string columnName)
            : base(columnName)
        {
            NumberStyles = NumberStyles.Integer;
        }

        /// <summary>
        /// Gets the type of the values in the column.
        /// </summary>
        public override Type ColumnType
        {
            get { return typeof(Int16); }
        }

        /// <summary>
        /// Gets or sets the format provider to use when parsing.
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets the number styles to use when parsing.
        /// </summary>
        public NumberStyles NumberStyles { get; set; }

        /// <summary>
        /// Gets or sets the format string to use when converting the value to a string.
        /// </summary>
        public string OutputFormat { get; set; }

        /// <summary>
        /// Parses the given value, returning an Int16.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <returns>The parsed Int16.</returns>
        public override object Parse(string value)
        {
            if (Preprocessor != null)
            {
                value = Preprocessor(value);
            }
            if (NullHandler.IsNullRepresentation(value))
            {
                return null;
            }
            IFormatProvider provider = FormatProvider ?? CultureInfo.CurrentCulture;
            value = TrimValue(value);
            return Int16.Parse(value, NumberStyles, provider);
        }

        /// <summary>
        /// Formats the given object.
        /// </summary>
        /// <param name="value">The object to format.</param>
        /// <returns>The formatted value.</returns>
        public override string Format(object value)
        {
            if (value == null)
            {
                return NullHandler.GetNullRepresentation();
            }

            short actual = (short)value;
            if (OutputFormat == null)
            {
                return actual.ToString(FormatProvider ?? CultureInfo.CurrentCulture);
            }
            else
            {
                return actual.ToString(OutputFormat, FormatProvider ?? CultureInfo.CurrentCulture);
            }
        }
    }
}
