namespace FlatFiles.Scrutinizer.Validators
{
    using System;
    using System.Collections.Generic;

    public class LengthChecker : IFilter<ResultDefinition>
    {
        public ResultDefinition Execute(ResultDefinition result)
        {
            try
            {
                if (result.Record.Length > result.MaxRecordLength || result.Record.Length < result.MaxRecordLength)
                {
                    result.Failures = new KeyValuePair<string, string>(result.RowIdentifier, $"<b>Length Error</b> Row Length did not meet the requirement length of <b>{result.MaxRecordLength}</b> but is <b>{result.Record.Length}</b>");
                    result.StopExecution = true;
                }
            }
            catch (Exception ex)
            {
                result.Failures = new KeyValuePair<string, string>(result.RowIdentifier, $"<b>Type Exception</b> An exception has occured while parsing {result.ColumnDefinition.ColumnName}. <br/> {ex.Message} <br/> {ex.StackTrace}");
                result.StopExecution = true;
            }

            return result;
        }
    }
}
