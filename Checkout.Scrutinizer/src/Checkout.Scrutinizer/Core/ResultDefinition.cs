namespace Checkout.Scrutinizer.Core
{
    using System.Collections.Generic;

    public class ResultDefinition
    {
        public string FilePath { get; set; }

        public string SchemaPath { get; set; }

        public int TotalLinesProcessed { get; set; }

        public string FileFormat { get; set; }

        public List<ValidationResult> Results { get; set; }
    }
}
