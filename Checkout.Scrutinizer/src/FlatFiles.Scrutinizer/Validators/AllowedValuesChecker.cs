namespace FlatFiles.Scrutinizer.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AllowedValuesChecker : IFilter<ResultDefinition>
    {
        public ResultDefinition Execute(ResultDefinition result)
        {
            Object parsedValue = null;
            try
            {
                if (result.Window.AllowedValues != null && result.Window.AllowedValues.Any())
                {
                    parsedValue = result.ColumnDefinition.Parse(result.RawValue);
                    result.ParsedValue = parsedValue;
                    if (!result.Window.AllowedValues.ContainsKey(parsedValue.ToString()))
                    {
                        var allowedValues = string.Format("({0})", string.Join(",", result.Window.AllowedValues.Keys));
                        result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Not found Error</b> Allowed values {allowedValues} doesn't contain value {parsedValue} for column {result.ColumnDefinition.ColumnName}");
                        result.StopExecution = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Not Found Exception</b> An exception has occured while parsing {result.ColumnDefinition.ColumnName}. <br/> {ex.Message} <br/> {ex.StackTrace}");
                result.StopExecution = true;
            }

            return result;
        }
    }
}
