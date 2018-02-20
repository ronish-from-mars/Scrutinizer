﻿using System;
using System.Globalization;
using System.Reflection;

namespace FlatFiles.Scrutinizer.TypeMapping
{
    /// <summary>
    /// Represents the mapping from a type property to a long column.
    /// </summary>
    public interface IUInt64PropertyMapping
    {
        /// <summary>
        /// Sets the name of the column in the input or output file.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IUInt64PropertyMapping ColumnName(string name);

        /// <summary>
        /// Sets the format provider to use.
        /// </summary>
        /// <param name="provider">The provider to use.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IUInt64PropertyMapping FormatProvider(IFormatProvider provider);

        /// <summary>
        /// Sets the number styles to expect when parsing the input.
        /// </summary>
        /// <param name="styles">The number styles used in the input.</param>
        /// <returns>The property mappig for further configuration.</returns>
        IUInt64PropertyMapping NumberStyles(NumberStyles styles);

        /// <summary>
        /// Sets the output format to use.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IUInt64PropertyMapping OutputFormat(string format);

        /// <summary>
        /// Sets the value to treat as null.
        /// </summary>
        /// <param name="value">The value to treat as null.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IUInt64PropertyMapping NullValue(string value);

        /// <summary>
        /// Sets a custom handler for nulls.
        /// </summary>
        /// <param name="handler">The handler to use to recognize nulls.</param>
        /// <returns>The property mapping for further configuration.</returns>
        /// <remarks>Setting the handler to null with use the default handler.</remarks>
        IUInt64PropertyMapping NullHandler(INullHandler handler);

        /// <summary>
        /// Sets a function to preprocess in the input before parsing it.
        /// </summary>
        /// <param name="preprocessor">A preprocessor function.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IUInt64PropertyMapping Preprocessor(Func<string, string> preprocessor);
    }

    internal sealed class UInt64PropertyMapping : IUInt64PropertyMapping, IMemberMapping
    {
        private readonly UInt64Column column;
        private readonly IMemberAccessor member;

        public UInt64PropertyMapping(UInt64Column column, IMemberAccessor member, int fileIndex, int workIndex)
        {
            this.column = column;
            this.member = member;
            this.FileIndex = fileIndex;
            this.WorkIndex = workIndex;
        }

        public IUInt64PropertyMapping ColumnName(string name)
        {
            this.column.ColumnName = name;
            return this;
        }

        public IUInt64PropertyMapping FormatProvider(IFormatProvider provider)
        {
            this.column.FormatProvider = provider;
            return this;
        }

        public IUInt64PropertyMapping NumberStyles(NumberStyles styles)
        {
            this.column.NumberStyles = styles;
            return this;
        }

        public IUInt64PropertyMapping OutputFormat(string format)
        {
            this.column.OutputFormat = format;
            return this;
        }

        public IUInt64PropertyMapping NullValue(string value)
        {
            this.column.NullHandler = new ConstantNullHandler(value);
            return this;
        }

        public IUInt64PropertyMapping NullHandler(INullHandler handler)
        {
            this.column.NullHandler = handler;
            return this;
        }

        public IUInt64PropertyMapping Preprocessor(Func<string, string> preprocessor)
        {
            this.column.Preprocessor = preprocessor;
            return this;
        }

        public IMemberAccessor Member
        {
            get { return member; }
        }

        public IColumnDefinition ColumnDefinition
        {
            get { return column; }
        }

        public int FileIndex { get; private set; }

        public int WorkIndex { get; private set; }
    }
}
