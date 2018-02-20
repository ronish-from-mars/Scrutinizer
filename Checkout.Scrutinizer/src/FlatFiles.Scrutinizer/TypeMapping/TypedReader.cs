﻿using System;
using System.Threading.Tasks;

namespace FlatFiles.Scrutinizer.TypeMapping
{
    /// <summary>
    ///  Represents a reader that will generate entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being read.</typeparam>
    public interface ITypedReader<out TEntity>
    {
        /// <summary>
        /// Gets the schema being used by the parser to parse record values.
        /// </summary>
        /// <returns>The schema being used by the parser.</returns>
        ISchema GetSchema();

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
        /// Gets the last read entity.
        /// </summary>
        TEntity Current { get; }
    }

    internal sealed class TypedReader<TEntity> : ITypedReader<TEntity>
    {
        private readonly IReader reader;
        private readonly Func<object[], TEntity> deserializer;
        private TEntity current;

        public TypedReader(IReader reader, IMapper<TEntity> mapper)
        {
            this.reader = reader;
            this.deserializer = mapper.GetReader();
        }

        public ISchema GetSchema()
        {
            return reader.GetSchema();
        }

        public bool Read()
        {
            if (!reader.Read())
            {
                return false;
            }
            object[] values = reader.GetValues();
            current = deserializer(values);
            return true;
        }

        public async ValueTask<bool> ReadAsync()
        {
            if (!await reader.ReadAsync())
            {
                return false;
            }
            object[] values = reader.GetValues();
            current = deserializer(values);
            return true;
        }

        public bool Skip()
        {
            return reader.Skip();
        }

        public async ValueTask<bool> SkipAsync()
        {
            return await reader.SkipAsync();
        }

        public TEntity Current
        {
            get { return current; }
        }
    }
}
