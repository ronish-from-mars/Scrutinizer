namespace FlatFiles.Scrutinizer.Validators
{
    using System;
    using System.Collections.Generic;

    public class AlignmentChecker : IFilter<ResultDefinition>
    {
        public ResultDefinition Execute(ResultDefinition result)
        {
            Object parsedValue = null;

            try
            {
                var alignment = result.Window.Alignment;
                var fillCharacter = Convert.ToChar(result.Window.FillCharacter);
                if (alignment != null && result.Window.FillCharacter != null)
                {
                    if (alignment == FixedAlignment.LeftAligned)
                    {
                        var rawValue = result.RawValue.TrimEnd(fillCharacter);
                        parsedValue = result.ColumnDefinition.Parse(rawValue);
                        var originalValue = parsedValue.ToString().PadRight(result.Window.Width, fillCharacter);
                        if (!originalValue.Equals(result.RawValue))
                        {
                            result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Padding/Alignment Error</b>: Padding or text alignment error on value <b>{rawValue}</b> for column <b>{result.ColumnDefinition.ColumnName}</b>");
                            result.StopExecution = true;
                        }
                        else
                        {
                            var trimEndValue = result.RawValue.TrimEnd(fillCharacter);
                            if (!trimEndValue.Equals(result.ParsedValue.ToString()))
                            {
                                result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Padding/Alignment Error</b>: Padding or text alignment error on value <b>{rawValue}</b> for column <b>{result.ColumnDefinition.ColumnName}</b>");
                                result.StopExecution = true;
                            }
                            else
                            {
                                var trimStartValue = rawValue.TrimStart(fillCharacter);
                                var padTrimStartValue = trimStartValue.PadRight(result.Window.Width, fillCharacter);
                                if (!padTrimStartValue.Equals(result.RawValue))
                                {
                                    result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Alignment Error</b>: Text alignment error on value <b>{rawValue}</b> for column <b>{result.ColumnDefinition.ColumnName}</b>. Alignment of text is expected on the left while padding character is expected on the right.");
                                    result.StopExecution = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        var rawValue = result.RawValue.TrimStart(fillCharacter);
                        parsedValue = result.ColumnDefinition.Parse(rawValue);
                        var originalValue = parsedValue.ToString().PadLeft(result.Window.Width, fillCharacter);
                        if (!originalValue.Equals(result.RawValue))
                        {
                            result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Padding/Alignment Error</b>: Padding or text alignment error on value <b>{rawValue}</b> for column <b>{result.ColumnDefinition.ColumnName}</b>");
                            result.StopExecution = true;
                        }
                        else
                        {
                            var trimStartValue = result.RawValue.TrimStart(fillCharacter);
                            if (!trimStartValue.Equals(result.ParsedValue.ToString()))
                            {
                                result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Padding/Alignment Error</b>: Padding or text alignment error on value <b>{rawValue}</b> for column <b>{result.ColumnDefinition.ColumnName}</b>");
                                result.StopExecution = true;
                            }
                            else
                            {
                                var trimEndValue = rawValue.TrimEnd(fillCharacter);
                                var padTrimEndValue = trimEndValue.PadLeft(result.Window.Width, fillCharacter);
                                if (!padTrimEndValue.Equals(result.RawValue))
                                {
                                    result.Failures = new KeyValuePair<string, string>(result.ColumnDefinition.ColumnName, $"<b>Alignment Error</b>: Text alignment error on value <b>{rawValue}</b> for column <b>{result.ColumnDefinition.ColumnName}</b>. Alignment of text is expected on the right while padding character is expected on the left.");
                                    result.StopExecution = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Failures = new KeyValuePair<string, string>(result.RowIdentifier, $"<b>Type Exception</b> An exception has occured while parsing {result.ColumnDefinition.ColumnName}. <br/> {ex.Message} <br/> {ex.StackTrace}");
                result.StopExecution = true;
            }
            result.ParsedValue = parsedValue;
            return result;
        }
    }
}
