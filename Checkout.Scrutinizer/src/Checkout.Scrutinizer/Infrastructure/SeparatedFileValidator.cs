namespace Checkout.Scrutinizer.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FlatFiles.Scrutinizer;
    using FlatFiles.Scrutinizer.Resources;

    using Checkout.Scrutinizer.Core;

    public class SeparatedFileValidator
    {
        public string FilePath { get; set; }

        public string SchemaFilePath { get; set; }

        public RawFileSchema RawSchema { get; set; }

        public string[] HtmlColors => new string[] { "#FFA07A", "#FF0000", "#FF4500" };

        public SeparatedFileValidator(string filePath, string schemaFilePath)
        {
            this.FilePath = filePath;
            this.SchemaFilePath = schemaFilePath;
        }

        public SeparatedFileValidator(string filePath, string schemaFilePath, RawFileSchema rawFileSchema)
        {
            this.FilePath = filePath;
            this.SchemaFilePath = schemaFilePath;
            this.RawSchema = rawFileSchema;
        }

        public Core.ResultDefinition ValidateFile()
        {
            var schemaData = string.Empty;
            var schemaParser = new SchemaParser();
            var validationResults = new List<ValidationResult>();
            var validator = new Validators.FixedLengthValidator();

            if (this.RawSchema == null)
            {
                try
                {
                    using (var sr = new StreamReader(File.OpenRead(this.SchemaFilePath)))
                    {
                        schemaData = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    var errorMsg = String.Format(null, SharedResources.SchemaError, ex.Message);
                    Console.WriteLine($"{errorMsg}");
                }

                this.RawSchema = schemaParser.ParseSchema(schemaData);
            }

            var separatedFileSchemaBuilder = new SeparatedSchemaBuilder();
            var separatedFileSchema = separatedFileSchemaBuilder.BuildSchema(this.RawSchema);
            var schemas = separatedFileSchema.SeparatedRecordSchemas;
            var count = 0;
            var options = new SeparatedValueOptions
            {
                Separator = separatedFileSchema.Delimeter,
                PreserveWhiteSpace = true
            };
            foreach (var inputLine in File.ReadLines(this.FilePath))
            {
                Console.WriteLine($"Started parsing line...{count}");
                
                if (inputLine != null)
                {
                    SeparatedValueSchema actualSchema;
                    TextReader stringReader = new StringReader(inputLine);
                    var schema = schemas.FirstOrDefault(x => inputLine.StartsWith(x.RecordIdentifier));
                    count++;
                    var innerResults = new List<ValidationResult>();
                    var line = inputLine;
                    if (schema != null)
                    {
                        var parser = new SeparatedValueReader(stringReader, schema.SeparatedValueSchema, options);

                        if (!inputLine.Contains(options.Separator))
                        {
                            var error = new ValidationResult
                            {
                                Record = inputLine,
                                ErrorMessages = new List<string>
                                {
                                    $"Column Separator Error: Record could not be parsed as separator {options.Separator} defined in schema is not found.",
                                },
                                HasErrors = true,
                                RowNumber = count,
                            };
                            validationResults.Add(error);

                            continue;
                        }

                        try
                        {
                            actualSchema = parser.GetSchema();
                            parser.Read();
                            var values = parser.GetValues();

                            for (int i = 0; i < values.Length; i++)
                            {
                                var validationResult = new ValidationResult
                                {
                                    Record = inputLine,
                                    ColumnName = actualSchema.ColumnDefinitions[i].ColumnName,
                                    ColumnType = actualSchema.ColumnDefinitions[i].ColumnType.Name,
                                    ActualRowLength = inputLine.Length,
                                    MaxColumns = actualSchema.ColumnDefinitions.HandledCount,
                                    ParsedValueLength = (values[i].ToString() ?? string.Empty).Length,
                                    RawValueLength = (values[i].ToString() ?? string.Empty).Length,
                                    ParsedValue = values[i].ToString() ?? string.Empty,
                                    RawValue = values[i].ToString() ?? string.Empty,
                                    RowNumber = count,
                                    ParsedValuesCount = values.Count()
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            var errorMsg = string.Empty;
                            if (ex.InnerException != null && ex.InnerException.Message != null)
                            {
                                errorMsg = ex.InnerException.Message;
                                Console.WriteLine(ex.InnerException.Message);
                            }
                            else
                            {
                                errorMsg = ex.Message;
                                Console.WriteLine(ex.Message);
                            }
                           
                            var error = new ValidationResult
                            {
                                RowNumber = count,
                                Record = inputLine,
                                ErrorMessages = new List<string>
                                {
                                    $"Exception: {errorMsg}"
                                },
                                HasErrors = true
                        };
                            validationResults.Add(error);
                        }
                    }
                    else
                    {
                        var error = new ValidationResult
                        {
                            RowNumber = count,
                            Record = inputLine,
                            ErrorMessages = new List<string>
                            {
                                    $"Exception: The row does not contain a record identifier that match the schema."
                            },
                            HasErrors = true
                        };
                        validationResults.Add(error);
                    }
                }
            }
            var results = new Core.ResultDefinition
            {
                Results = validationResults,
                TotalLinesProcessed = count,
                FileFormat = this.RawSchema.FileFormat
            };
            return results;
        }
    }
}
