﻿using System;
using System.Reflection;

namespace FlatFiles.Scrutinizer.TypeMapping
{
    /// <summary>
    /// Represents the mapping from a type property to a Boolean column.
    /// </summary>
    public interface IIgnoredMapping
    {
        /// <summary>
        /// Sets the name of the column in the input or output file.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The property mapping for further configuration.</returns>
        IIgnoredMapping ColumnName(string name);
    }

    internal sealed class IgnoredMapping : IIgnoredMapping, IMemberMapping
    {
        private readonly IgnoredColumn column;

        public IgnoredMapping(IgnoredColumn column, int fileIndex)
        {
            this.column = column;
            this.FileIndex = fileIndex;
        }

        public IIgnoredMapping ColumnName(string name)
        {
            this.column.ColumnName = name;
            return this;
        }

        public IMemberAccessor Member
        {
            get { return null; }
        }

        public IColumnDefinition ColumnDefinition
        {
            get { return column; }
        }

        public int FileIndex { get; private set; }

        public int WorkIndex
        {
            get { return -1; }
        }
    }
}
