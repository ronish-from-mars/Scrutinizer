namespace Checkout.Scrutinizer.Infrastructure
{
    using FlatFiles.Scrutinizer;

    using Checkout.Scrutinizer.Core;

    public class SeparatedRecordSchema : RecordSchemaBase
    {
        public SeparatedValueSchema SeparatedValueSchema { get; set; }
    }
}
