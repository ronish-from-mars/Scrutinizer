using System.Collections.Generic;

namespace Checkout.Scrutinizer.Core
{
    public class ColumnDefinition
    {
        public string Name { get; set; }

        public string DataType { get; set; }

        public int? Length { get; set; }

        public string Alignment { get; set; }

        public string PadChar { get; set; }

        public string NullValue { get; set; }

        public bool? TruncateIfExceedFieldLength { get; set; }

        public string InputFormat { get; set; }

        public string OutputFormat { get; set; }

        public string Format { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }

        public IEnumerable<ColumnDefinition> AllowedValues { get; set; }
    }
}
