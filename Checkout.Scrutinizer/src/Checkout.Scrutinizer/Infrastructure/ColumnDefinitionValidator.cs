namespace Checkout.Scrutinizer.Infrastructure
{
    using FluentValidation;

    using Checkout.Scrutinizer.Core;

    public class ColumnDefinitionValidator: AbstractValidator<ColumnDefinition>
    {
        public ColumnDefinitionValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Name).NotNull().WithMessage("Column 'Name' must be present");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Column 'Name' cannot be empty");

            RuleFor(x => x.DataType).NotNull().WithMessage("Column 'DataType' must be present");
            RuleFor(x => x.DataType).NotEmpty().WithMessage("Column 'DataType' cannot be empty");
        }
    }
}