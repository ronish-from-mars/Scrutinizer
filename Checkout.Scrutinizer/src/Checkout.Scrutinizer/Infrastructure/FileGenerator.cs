namespace Checkout.Scrutinizer.Infrastructure
{
    using System;
    using System.IO;
    using System.Linq;

    using Checkout.Scrutinizer.Core;
    using Checkout.Scrutinizer.Core.Extensions;

    public class FileGenerator : IFileGenerator
    {
        private readonly ISchemaValidator _schemaValidator;
        private readonly IFileWriter _fileWriter;
        private readonly string _rootFolder;

        public FileGenerator(ISchemaValidator schemaValidator,
                             IFileWriter fileWriter,
                             string rootFolder)
        {
            _schemaValidator = schemaValidator;
            _fileWriter = fileWriter;
            _rootFolder = rootFolder;
        }

        public void Generate(RawFileSchema rawFileSchema, DataSource dataSource)
        {
            var validationErrors = _schemaValidator.ValidateSchema(rawFileSchema);

            if (validationErrors.Any())
            {
                throw new Exception("Errors");
            }

            var fileFormat = rawFileSchema.FileFormat.ToEnum<FileSchemaType>();

            var rootDirectory = Path.Combine(_rootFolder, rawFileSchema.RootOutputDir);

            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }

            var generatedFilePath = Path.Combine(rootDirectory, rawFileSchema.OutputFileName);

            if (!string.IsNullOrWhiteSpace(rawFileSchema.OutputFileExtension))
            {
                generatedFilePath += rawFileSchema.OutputFileExtension;
            }

            switch (fileFormat)
            {
                case FileSchemaType.Fixed:
                    var fixedLengthFileSchemaBuilder = new FixedLengthSchemaBuilder();
                    var fixedFileSchema = fixedLengthFileSchemaBuilder.BuildSchema(rawFileSchema);
                    _fileWriter.Write(generatedFilePath, dataSource, fixedFileSchema);
                    break;

                case FileSchemaType.Separated:
                    var separatedFileSchemaBuilder = new SeparatedSchemaBuilder();
                    var separatedFileSchema = separatedFileSchemaBuilder.BuildSchema(rawFileSchema);
                    _fileWriter.Write(generatedFilePath, dataSource, separatedFileSchema);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

    }
}
