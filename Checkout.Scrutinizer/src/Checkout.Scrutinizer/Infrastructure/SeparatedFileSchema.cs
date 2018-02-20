namespace Checkout.Scrutinizer.Infrastructure
{
    using System.Collections.Generic;

    using Checkout.Scrutinizer.Core;

    public class SeparatedFileSchema : FileSchemaBase
    {
        public string Delimeter { get; set; }

        public List<SeparatedRecordSchema> SeparatedRecordSchemas { get; set; } = new List<SeparatedRecordSchema>();
    }
}
