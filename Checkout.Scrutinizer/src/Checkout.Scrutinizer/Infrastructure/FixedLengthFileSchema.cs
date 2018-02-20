namespace Checkout.Scrutinizer.Infrastructure
{
    using System.Collections.Generic;

    using Checkout.Scrutinizer.Core;

    public class FixedLengthFileSchema : FileSchemaBase
    {
        public List<FixedLengthRecordSchema> FixedLengthRecordSchemas { get; set; } = new List<FixedLengthRecordSchema>();
    }
}
