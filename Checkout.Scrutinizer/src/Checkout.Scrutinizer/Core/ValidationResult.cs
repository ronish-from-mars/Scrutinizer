namespace Checkout.Scrutinizer.Core
{
    using System.Collections.Generic;

    public class ValidationResult
    {
        public string ColumnName { get; set; }

        public string ColumnType { get; set; }

        public string AttemptedValue { get; set; }

        public string Record { get; set; }

        public string RawValue { get; set; }

        public object ParsedValue { get; set; }

        public int RawValueLength { get; set; }

        public int ParsedValueLength { get; set; }

        public int ColumnLength { get; set; }

        public string TextAlignment { get; set; }

        public string FillCharacter { get; set; }

        public int ActualRowLength { get; set; }

        public int MaxRowLength { get; set; }

        public int RowNumber { get; set; }

        public string RowIdentifier => $"Row {RowNumber}";

        public string Format { get; set; }

        public Dictionary<string, string> AllowedValues { get; set; }

        public string  ErrorMessage { get; set; }

        public List<string> ErrorMessages { get; set; }

        public bool HasErrors { get; set; }

        public int MaxColumns { get; set; }

        public int ParsedValuesCount { get; set; }
    }
}
