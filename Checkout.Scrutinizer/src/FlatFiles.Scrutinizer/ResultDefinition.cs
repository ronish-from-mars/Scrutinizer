using FlatFiles.Scrutinizer.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatFiles.Scrutinizer
{
    public class ResultDefinition : IResult
    {
        public string RawValue { get; set; }

        public Object ParsedValue { get; set; }

        public string Record { get; set; }

        public IColumnDefinition ColumnDefinition { get; set; }

        public Window Window { get; set; }

        public int MaxRecordLength { get; set; }

        public int RowNumber { get; set; }

        public string RowIdentifier => $"Row {RowNumber}";

        public bool StopExecution { get; set; }

        public KeyValuePair<string, string> Failures { get; set; }

    }
}
