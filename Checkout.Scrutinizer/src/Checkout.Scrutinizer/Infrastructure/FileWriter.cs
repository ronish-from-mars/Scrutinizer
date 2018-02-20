namespace Checkout.Scrutinizer.Infrastructure
{
    using System;
    using System.IO;
    using System.Linq;

    using FlatFiles.Scrutinizer;

    using Checkout.Scrutinizer.Core;

    public class FileWriter: IFileWriter
    {
        public void Write(string outputFilePath, DataSource dataSource, FileSchemaBase schema)
        {
            using (var fileStreamWriter = new StreamWriter(outputFilePath, true))
            {
                switch (schema)
                {
                    case FixedLengthFileSchema fixedLengthFileSchema:
                        foreach (var rowData in dataSource.Data)
                        {
                            var rowSchema = fixedLengthFileSchema.FixedLengthRecordSchemas.FirstOrDefault(x => x.RecordIdentifier.Equals(rowData.Identifier));

                            if (rowSchema == null)
                            {
                                throw new Exception($"No such identifier defined: '{rowData.Identifier}'");
                            }

                            var fixedLengthWriter = new FixedLengthWriter(fileStreamWriter, rowSchema.FixedLengthSchema);
                            fixedLengthWriter.Write(rowData.DataArray);
                        }
                        break;

                    case SeparatedFileSchema separatedFileSchema:
                        foreach (var rowData in dataSource.Data)
                        {
                            var rowSchema = separatedFileSchema.SeparatedRecordSchemas.FirstOrDefault(x => x.RecordIdentifier.Equals(rowData.Identifier));

                            if (rowSchema == null)
                            {
                                throw new Exception($"No such identifier defined: '{rowData.Identifier}'");
                            }

                            var separatedWriter = new SeparatedValueWriter(fileStreamWriter, rowSchema.SeparatedValueSchema, new SeparatedValueOptions
                            {
                                Separator = separatedFileSchema.Delimeter
                            });

                            separatedWriter.Write(rowData.DataArray);
                        }
                        break;
                }

            }
        }
    }
}
