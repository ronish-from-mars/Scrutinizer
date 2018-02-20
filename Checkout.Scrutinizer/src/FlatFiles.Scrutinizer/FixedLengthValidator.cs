namespace FlatFiles.Scrutinizer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FlatFiles.Scrutinizer.Resources;
    using FlatFiles.Scrutinizer.Validators;

    /// <summary>
    /// Extracts records from a file that has value in fixed-length columns.
    /// </summary>
    public sealed class FixedLengthValidator : IReader
    {
        private readonly FixedLengthRecordParser parser;
        private readonly FixedLengthSchema schema;
        private readonly FixedLengthOptions options;
        private int recordCount;
        private object[] values;
        private IEnumerable<ResultDefinition> validationResults;
        private bool endOfFile;
        private bool hasError;
        private int rowNumber;

        /// <summary>
        /// Initializes a new FixedLengthReader with the given schema.
        /// </summary>
        /// <param name="reader">A reader over the fixed-length document.</param>
        /// <param name="schema">The schema of the fixed-length document.</param>
        /// <param name="options">The options controlling how the fixed-length document is read.</param>
        /// <exception cref="ArgumentNullException">The reader is null.</exception>
        /// <exception cref="ArgumentNullException">The schema is null.</exception>
        public FixedLengthValidator(TextReader reader, FixedLengthSchema schema, int row, FixedLengthOptions options = null)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (schema == null)
            {
                throw new ArgumentNullException("schema");
            }
            if (options == null)
            {
                options = new FixedLengthOptions();
            }
            parser = new FixedLengthRecordParser(reader, schema, options);
            this.schema = schema;
            this.options = options.Clone();
            this.rowNumber = row;
        }

        /// <summary>
        /// Gets the schema being used by the parser.
        /// </summary>
        /// <returns>The schema being used by the parser.</returns>
        public FixedLengthSchema GetSchema()
        {
            return schema;
        }

        ISchema IReader.GetSchema()
        {
            return GetSchema();
        }

        /// <summary>
        /// Gets the schema being used by the parser.
        /// </summary>
        /// <returns>The schema being used by the parser.</returns>
        public Task<FixedLengthSchema> GetSchemaAsync()
        {
            return Task.FromResult(schema);
        }

        Task<ISchema> IReader.GetSchemaAsync()
        {
            return Task.FromResult<ISchema>(schema);
        }

  
        public bool Read()
        {
            if (hasError)
            {
                throw new InvalidOperationException(SharedResources.ReadingWithErrors);
            }
            handleHeader();
            try
            {
                validationResults = partitionWithFilter();
                return validationResults != null;
            }
            catch (FlatFileException)
            {
                hasError = true;
                throw;
            }
        }

        private void handleHeader()
        {
            if (recordCount == 0 && options.IsFirstRecordHeader)
            {
                skip();
            }
        }

        private IEnumerable<ResultDefinition> partitionWithFilter()
        {
            string record = readWithFilter();
            return partitionRecord(record);
        }

        private string readWithFilter()
        {
            string record = readNextRecord();
            while (record != null && options.UnpartitionedRecordFilter != null && options.UnpartitionedRecordFilter(record))
            {
                record = readNextRecord();
            }
            return record;
        }

        private async Task handleHeaderAsync()
        {
            if (recordCount == 0 && options.IsFirstRecordHeader)
            {
                await skipAsync();
            }
        }

        private async Task<IEnumerable<ResultDefinition>> partitionWithFilterAsync()
        {
            string record = await readWithFilterAsync();
            return partitionRecord(record);
        }

        private async Task<string> readWithFilterAsync()
        {
            string record = await readNextRecordAsync();
            while (record != null && options.UnpartitionedRecordFilter != null && options.UnpartitionedRecordFilter(record))
            {
                record = await readNextRecordAsync();
            }
            return record;
        }

        private object[] parseValues(string[] rawValues)
        {
            try
            {
                return schema.ParseValues(rawValues);
            }
            catch (FlatFileException exception)
            {
                processError(new RecordProcessingException(recordCount, SharedResources.InvalidRecordConversion, exception));
                return null;
            }
        }

        /// <summary>
        /// Skips the next record from the file.
        /// </summary>
        /// <returns>True if the next record was skipped; otherwise, false if all records are read.</returns>
        /// <remarks>The previously parsed values remain available.</remarks>
        public bool Skip()
        {
            if (hasError)
            {
                throw new InvalidOperationException(SharedResources.ReadingWithErrors);
            }
            handleHeader();
            return skip();
        }

        private bool skip()
        {
            string record = readNextRecord();
            return record != null;
        }

        /// <summary>
        /// Skips the next record from the file.
        /// </summary>
        /// <returns>True if the next record was skipped; otherwise, false if all records are read.</returns>
        /// <remarks>The previously parsed values remain available.</remarks>
        public async ValueTask<bool> SkipAsync()
        {
            if (hasError)
            {
                throw new InvalidOperationException(SharedResources.ReadingWithErrors);
            }
            await handleHeaderAsync();
            return await skipAsync();
        }

        private async ValueTask<bool> skipAsync()
        {
            string record = await readNextRecordAsync();
            return record != null;
        }

        private IEnumerable<ResultDefinition> partitionRecord(string record)
        {
            IEnumerable<ResultDefinition> validationResults = new List<ResultDefinition>();

            var outerPipeline = new Pipeline<ResultDefinition>();
            outerPipeline.Register(new LengthChecker());

            var rowDefinition = new ResultDefinition
            {
                MaxRecordLength = schema.TotalWidth,
                RowNumber = rowNumber,
                Record = record,
            };

            var result = outerPipeline.Execute(rowDefinition);
            if(result != null && result.Any())
            {
                return result;
            }
           
            var innerPipeline = new Pipeline<ResultDefinition>();
            innerPipeline.Register(new TypeChecker());
            innerPipeline.Register(new AlignmentChecker());
            innerPipeline.Register(new AllowedValuesChecker());

            WindowCollection windows = schema.Windows;
            ColumnCollection definitions = schema.ColumnDefinitions;
            string[] values = new string[windows.Count];

            int offset = 0;

            for (int index = 0; index != values.Length; ++index)
            {
                Window window = windows[index];
                IColumnDefinition definition = definitions[index];
                string value = record.Substring(offset, window.Width);

                values[index] = value;
                offset += window.Width;
                var dataDefinition = new ResultDefinition
                {
                    RawValue = value,
                    ColumnDefinition = definition,
                    Window = window,
                    MaxRecordLength = schema.TotalWidth,
                    RowNumber = rowNumber,
                    Record = record
                };
                var innerResult = innerPipeline.Execute(dataDefinition);
                if (innerResult != null && innerResult.Any())
                {
                    validationResults = validationResults.Concat(innerResult);
                }

            }
            return validationResults;
        }

        private string readNextRecord()
        {
            if (parser.IsEndOfStream())
            {
                endOfFile = true;
                return null;
            }
            string record = parser.ReadRecord();
            ++recordCount;
            return record;
        }

        private async Task<string> readNextRecordAsync()
        {
            if (await parser.IsEndOfStreamAsync())
            {
                endOfFile = true;
                return null;
            }
            string record = await parser.ReadRecordAsync();
            ++recordCount;
            return record;
        }

        private void processError(RecordProcessingException exception)
        {
            if (options.ErrorHandler != null)
            {
                var args = new ProcessingErrorEventArgs(exception);
                options.ErrorHandler(this, args);
                if (args.IsHandled)
                {
                    return;
                }
            }
            throw exception;
        }

        /// <summary>
        /// Gets the values for the current record.
        /// </summary>
        /// <returns>The values of the current record.</returns>
        public IEnumerable<ResultDefinition> GetValidationResults()
        {
            if (hasError)
            {
                throw new InvalidOperationException(SharedResources.ReadingWithErrors);
            }
            if (recordCount == 0)
            {
                throw new InvalidOperationException(SharedResources.ReadNotCalled);
            }
            if (endOfFile)
            {
                throw new InvalidOperationException(SharedResources.NoMoreRecords);
            }
            return validationResults;
        }

     
        public ValueTask<bool> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public object[] GetValues()
        {
            throw new NotImplementedException();
        }
    }
}
