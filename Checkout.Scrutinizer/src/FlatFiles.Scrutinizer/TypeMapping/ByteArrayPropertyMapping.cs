﻿using System;
using System.Reflection;
using System.Text;

namespace FlatFiles.Scrutinizer.TypeMapping
{
    /// <summary>
    /// Represents the mapping from a type property to a byte array column.
    /// </summary>
    public interface IByteArrayPropertyMapping
    {
        /// <summary>
        /// Sets the name of the column in the input or output file.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IByteArrayPropertyMapping ColumnName(string name);

        /// <summary>
        /// Sets the encoding to use to read and write the column.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IByteArrayPropertyMapping Encoding(Encoding encoding);

        /// <summary>
        /// Sets the value to treat as null.
        /// </summary>
        /// <param name="value">The value to treat as null.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IByteArrayPropertyMapping NullValue(string value);

        /// <summary>
        /// Sets a custom handler for nulls.
        /// </summary>
        /// <param name="handler">The handler to use to recognize nulls.</param>
        /// <returns>The property mapping for further configuration.</returns>
        /// <remarks>Setting the handler to null with use the default handler.</remarks>
        IByteArrayPropertyMapping NullHandler(INullHandler handler);

        /// <summary>
        /// Sets a function to preprocess in the input before parsing it.
        /// </summary>
        /// <param name="preprocessor">A preprocessor function.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IByteArrayPropertyMapping Preprocessor(Func<string, string> preprocessor);
    }

    internal sealed class ByteArrayPropertyMapping : IByteArrayPropertyMapping, IMemberMapping
    {
        private readonly ByteArrayColumn column;
        private readonly IMemberAccessor member;

        public ByteArrayPropertyMapping(ByteArrayColumn column, IMemberAccessor member, int fileIndex, int workIndex)
        {
            this.column = column;
            this.member = member;
            this.FileIndex = fileIndex;
            this.WorkIndex = workIndex;
        }

        public IByteArrayPropertyMapping ColumnName(string name)
        {
            this.column.ColumnName = name;
            return this;
        }

        public IByteArrayPropertyMapping Encoding(Encoding encoding)
        {
            this.column.Encoding = encoding;
            return this;
        }

        public IByteArrayPropertyMapping NullValue(string value)
        {
            this.column.NullHandler = new ConstantNullHandler(value);
            return this;
        }

        public IByteArrayPropertyMapping NullHandler(INullHandler handler)
        {
            this.column.NullHandler = handler;
            return this;
        }

        public IByteArrayPropertyMapping Preprocessor(Func<string, string> preprocessor)
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
