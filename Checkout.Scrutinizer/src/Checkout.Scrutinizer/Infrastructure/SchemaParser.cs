namespace Checkout.Scrutinizer.Infrastructure
{
    using Checkout.Scrutinizer.Core;
    using Checkout.Scrutinizer.Infrastructure.Extensions;

    public class SchemaParser : ISchemaParser
    {
        public RawFileSchema ParseSchema(string jsonData)
        {
            return jsonData.Deserialize<RawFileSchema>();
        }
    }
}
