namespace Checkout.Scrutinizer.Infrastructure.Validators
{
    using FlatFiles.Scrutinizer;
    using FluentValidation;
    using FluentValidation.Results;
    using System;

    public class AlignmentValidator : AbstractValidator<Core.ValidationResult>
    {
        public AlignmentValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(c => c).Custom((column, context) =>
            {
                if (column.FillCharacter != null && column.FillCharacter != "Not specified")
                {
                    var fillCharacter = Convert.ToChar(column.FillCharacter);

                    #region Replacing empty strings
                    if (column.FillCharacter == string.Empty)
                    {
                        column.FillCharacter = "[blank]";
                    }
                    else if (string.IsNullOrWhiteSpace(column.FillCharacter))
                    {
                        column.FillCharacter = column.FillCharacter.Replace(" ", "[space]");
                    }
                    else if ((column.FillCharacter.StartsWith(" ") || column.FillCharacter.EndsWith(" ")) && column.FillCharacter != "Not specified")
                    {
                        column.FillCharacter = column.FillCharacter.Replace(" ", "[space]");
                    }
                    #endregion

                    if (column.TextAlignment == FixedAlignment.LeftAligned.ToString())
                    {
                        var expectedValue = column.ParsedValue.ToString().PadRight(column.ColumnLength, fillCharacter);
                        if (!expectedValue.Equals(column.RawValue))
                        {
                            context.AddFailure(new ValidationFailure(column.ColumnName, $"Alignment or Padding Error: Padding or text alignment error on value {column.RawValue} for column {column.ColumnName}")
                            {
                                ResourceName = column.RowIdentifier
                            });
                        }
                        else
                        {
                            var trimEndValue = column.RawValue.TrimEnd(fillCharacter);
                            if (!trimEndValue.Equals(column.ParsedValue.ToString()))
                            {
                                context.AddFailure(new ValidationFailure(column.ColumnName, $"Alignment or Padding Error: Padding or text alignment error on value {column.RawValue} for column {column.ColumnName}.")
                                {
                                    ResourceName = column.RowIdentifier
                                });
                            }
                            else
                            {
                                var trimStartValue = column.RawValue.TrimStart(fillCharacter);
                                var padTrimStartValue = trimStartValue.PadRight(column.ColumnLength, fillCharacter);

                                if (!padTrimStartValue.Equals(column.RawValue))
                                {
                                    context.AddFailure(new ValidationFailure(column.ColumnName, $"Alignment Error: Text alignment error on value {column.RawValue} for column {column.ColumnName}. Alignment of text is expected on the left while padding character is expected on the right.")
                                    {
                                        ResourceName = column.RowIdentifier
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        var expectedValue = column.ParsedValue.ToString().PadLeft(column.ColumnLength, fillCharacter);
                        if (!expectedValue.Equals(column.RawValue))
                        {
                            context.AddFailure(new ValidationFailure(column.ColumnName, $"Alignment or Padding Error: Padding or text alignment error on value {column.RawValue} for column {column.ColumnName}.")
                            {
                                ResourceName = column.RowIdentifier
                            });
                        }
                        else
                        {

                            var trimStartValue = column.RawValue.TrimStart(fillCharacter);
                            if (!trimStartValue.Equals(column.ParsedValue.ToString()))
                            {
                                context.AddFailure(new ValidationFailure(column.ColumnName, $"Alignment or Padding Error: Padding or text alignment error on value {column.RawValue} for column {column.ColumnName}.")
                                {
                                    ResourceName = column.RowIdentifier
                                });
                            }
                            else
                            {
                                var trimEndValue = column.RawValue.TrimEnd(fillCharacter);
                                var padTrimStartValue = trimStartValue.PadLeft(column.ColumnLength, fillCharacter);
                                if (!padTrimStartValue.Equals(column.RawValue))
                                {
                                    context.AddFailure(new ValidationFailure(column.ColumnName, $"Alignment Error: Text alignment error on value {column.RawValue} for column {column.ColumnName}. Alignment of text is expected on the right while padding character is expected on the left.")
                                    {
                                        ResourceName = column.RowIdentifier
                                    });
                                }
                            }

                        }
                    }
                }
            });
        }
    }
}
