namespace FlatFiles.Scrutinizer.Validators
{
    using System;
    using System.Collections.Generic;

    public class TypeChecker : IFilter<ResultDefinition>
    {
        public ResultDefinition Execute(ResultDefinition result)
        {
            Object parsedValue = null;
            try
            {
                parsedValue = result.ColumnDefinition.Parse(result.RawValue);
                result.ParsedValue = parsedValue;
                if (parsedValue.GetType() != result.ColumnDefinition.ColumnType)
                {
                    result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Type Error</b> {result.ColumnDefinition.ColumnName} is of type {result.ColumnDefinition.ColumnType}");
                    result.StopExecution = true;
                }
            }
            catch (Exception ex)
            {
                result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Type Exception</b> An exception has occured while parsing {result.ColumnDefinition.ColumnName}. <br/> {ex.Message} <br/> {ex.StackTrace}");
                result.StopExecution = true;
            }

            return result;
        }
    }
}
