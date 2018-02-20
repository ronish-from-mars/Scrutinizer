namespace Checkout.Scrutinizer.Infrastructure
{
    using Checkout.Scrutinizer.Core;

    public interface ISchemaParser
    {
        RawFileSchema ParseSchema(string jsonData);
    }
}
