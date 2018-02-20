namespace Checkout.Scrutinizer.Infrastructure.Validators
{
    using FluentValidation;
    using FluentValidation.Results;
    using System.Linq;

    public class AllowedValuesValidator : AbstractValidator<Core.ValidationResult>
    {
        public AllowedValuesValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(c => c).Custom((column, context) =>
            {
                if (column.AllowedValues != null && column.AllowedValues.Any())
                {
                    if (!column.AllowedValues.ContainsKey(column.ParsedValue.ToString()))
                    {
                        var allowedValues = string.Format("({0})", string.Join(",", column.AllowedValues.Keys));
                        context.AddFailure(new ValidationFailure(column.ColumnName, $"Not found error: Allowed values {allowedValues} doesn't contain value {column.ParsedValue} for column {column.ColumnName}")
                        {
                            ResourceName = column.RowIdentifier
                        });
                    }
                }
            });
        }
    }
}
