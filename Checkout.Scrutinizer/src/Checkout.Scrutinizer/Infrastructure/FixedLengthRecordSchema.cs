namespace Checkout.Scrutinizer.Infrastructure
{
    using FlatFiles.Scrutinizer;

    using Checkout.Scrutinizer.Core;

    public class FixedLengthRecordSchema : RecordSchemaBase
    {
        public FixedLengthSchema FixedLengthSchema { get; set; }
    }
}
