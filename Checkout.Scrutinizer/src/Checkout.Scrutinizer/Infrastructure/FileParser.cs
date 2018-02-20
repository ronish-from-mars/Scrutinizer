using System;
using System.IO;
using System.Linq;
using Checkout.Scrutinizer.Core;
using Checkout.Scrutinizer.Core.Extensions;

namespace Checkout.Scrutinizer.Infrastructure
{
    public class FileParser
    {
        private readonly ISchemaValidator _schemaValidator;

        public FileParser(ISchemaValidator schemaValidator)
        {
            _schemaValidator = schemaValidator;
        }

        public void ParseFile(RawFileSchema rawFileSchema)
        {
            var validationErrors = _schemaValidator.ValidateSchema(rawFileSchema);

            if (validationErrors.Any())
            {
                throw new Exception("Errors");
            }

            var fileFormat = rawFileSchema.FileFormat.ToEnum<FileSchemaType>();

            var generatedFilePath = Path.Combine(rawFileSchema.RootOutputDir, rawFileSchema.OutputFileName);

            if (!string.IsNullOrWhiteSpace(rawFileSchema.OutputFileExtension))
            {
                generatedFilePath += rawFileSchema.OutputFileExtension;
            }

            switch (fileFormat)
            {
                case FileSchemaType.Fixed:
                    var fixedLengthFileSchemaBuilder = new FixedLengthSchemaBuilder();
                    var fixedFileSchema = fixedLengthFileSchemaBuilder.BuildSchema(rawFileSchema);

                    using (var sr = new StreamReader(File.OpenRead(generatedFilePath)))
                    {
                        //var fixedLengthSchemaReader = new FlatFiles.FixedLengthReader(sr, );
                    }
                    break;

                case FileSchemaType.Separated:
                    var separatedFileSchemaBuilder = new SeparatedSchemaBuilder();
                    var separatedFileSchema = separatedFileSchemaBuilder.BuildSchema(rawFileSchema);
                    //_fileWriter.Write(generatedFilePath, dataSource, separatedFileSchema);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
        }
    
}
