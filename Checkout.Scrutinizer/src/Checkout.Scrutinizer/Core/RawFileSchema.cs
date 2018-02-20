namespace Checkout.Scrutinizer.Core
{
    public class RawFileSchema
    {
        public string SchemaType { get; set; }

        public string RootOutputDir { get; set; }

        public string OutputFileName { get; set; }

        public string OutputFileExtension { get; set; }

        public string FileFormat { get; set; }

        public string Delimeter { get; set; }

        public DataSchema Schema { get; set; }
    }
}
