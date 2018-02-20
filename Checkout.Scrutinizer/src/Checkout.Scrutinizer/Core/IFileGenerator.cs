namespace Checkout.Scrutinizer.Core
{
    public interface IFileGenerator
    {
        void Generate(RawFileSchema rawFileSchema, DataSource dataSource);
    }
}
