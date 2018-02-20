namespace Checkout.Scrutinizer.Core
{
    public interface IFileWriter
    {
        void Write(string outputFilePath, DataSource dataSource, FileSchemaBase schema);
    }
}