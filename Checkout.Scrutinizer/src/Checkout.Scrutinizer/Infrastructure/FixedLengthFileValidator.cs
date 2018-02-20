namespace Checkout.Scrutinizer.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FlatFiles.Scrutinizer;
    using FlatFiles.Scrutinizer.Resources;

    using Checkout.Scrutinizer.Core;

    public class FixedLengthFileValidator
    {
        public string FilePath { get; set; }

        public string SchemaFilePath { get; set; }

        public RawFileSchema RawSchema { get; set; }

        public string[] HtmlColors => new string[] { "#FFA07A", "#FF0000", "#FF4500" };

        public FixedLengthFileValidator(string filePath, string schemaFilePath)
        {
            this.FilePath = filePath;
            this.SchemaFilePath = schemaFilePath;
        }

        public FixedLengthFileValidator(string filePath, string schemaFilePath, RawFileSchema rawFileSchema)
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

            var fixedLengthFileSchemaBuilder = new FixedLengthSchemaBuilder();
            var fixedFileSchema = fixedLengthFileSchemaBuilder.BuildSchema(this.RawSchema);
            var schemas = fixedFileSchema.FixedLengthRecordSchemas;
            var count = 0;
            foreach (var inputLine in File.ReadLines(this.FilePath))
            {
                Console.WriteLine($"Started parsing line...{count}");
                
                if (inputLine != null)
                {
                    FixedLengthSchema actualSchema;
                    TextReader stringReader = new StringReader(inputLine);
                    var schema = schemas.FirstOrDefault(x => inputLine.StartsWith(x.RecordIdentifier));
                    count++;
                    var innerResults = new List<ValidationResult>();
                    var line = inputLine;
                    if (schema != null)
                    {
                        var parser = new FixedLengthReader(stringReader, schema.FixedLengthSchema);

                        try
                        {
                            actualSchema = parser.GetSchema();
                            parser.Read();
                            var rawValues = GetRawValues(inputLine, schema.FixedLengthSchema);
                            var values = parser.GetValues();

                            for (int i = 0; i < values.Length; i++)
                            {
                                var validationResult = new ValidationResult
                                {
                                    Record = inputLine,
                                    ColumnName = actualSchema.ColumnDefinitions[i].ColumnName,
                                    ColumnType = actualSchema.ColumnDefinitions[i].ColumnType.Name,
                                    ActualRowLength = inputLine.Length,
                                    MaxRowLength = actualSchema.TotalWidth,
                                    ParsedValueLength = (values[i].ToString() ?? string.Empty).Length,
                                    RawValueLength = rawValues[i].ToString().Length,
                                    ColumnLength = actualSchema.Windows[i].Width,
                                    FillCharacter = actualSchema.Windows[i].FillCharacter != null ? actualSchema.Windows[i].FillCharacter.ToString() : "Not specified",
                                    TextAlignment = actualSchema.Windows[i].Alignment != null ? actualSchema.Windows[i].Alignment.ToString() : "Not specified",
                                    ParsedValue = values[i].ToString() ?? string.Empty,
                                    RawValue = rawValues[i].ToString() ?? string.Empty,
                                    RowNumber = count
                                };
                                var failures = validator.Validate(validationResult);
                                if (failures.Errors != null && failures.Errors.Any() && !failures.IsValid)
                                {
                                    var errors = new List<string>();
                                    foreach (var error in failures.Errors)
                                    {
                                        errors.Add(error.ErrorMessage);
                                    }
                                    validationResult.ErrorMessages = errors;
                                    validationResult.HasErrors = true;
                                    line = line.Replace(validationResult.RawValue, $"<strong style='color:{HtmlColors[i%HtmlColors.Length]}'>{validationResult.RawValue}</strong>");
                                    validationResult.Record = line;
                                    validationResults.Add(validationResult);
                                }
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
                FileFormat = this.RawSchema.FileFormat,
                FilePath = this.FilePath,
                SchemaPath = this.SchemaFilePath
            };
            return results;
        }

        public string[] GetRawValues(string record, FixedLengthSchema schema)
        {
            if (record == null)
            {
                return null;
            }
            var windows = schema.Windows;
            var values = new string[windows.Count];
            int offset = 0;
            for (int index = 0; index != values.Length; ++index)
            {
                var window = windows[index];
                string value = record.Substring(offset, window.Width);
                values[index] = value;
                offset += window.Width;
            }
            return values;
        }
    }
}
