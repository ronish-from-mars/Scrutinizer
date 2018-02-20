namespace Checkout.Scrutinizer.Infrastructure
{
    using System.Linq;

    using FluentValidation;

    using Checkout.Scrutinizer.Core;

    public class RowDefinitionValidator: AbstractValidator<RowDefinition>
    {
        public RowDefinitionValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Identifier).NotNull().WithMessage("Row 'Identifier' must be present.");
            RuleFor(x => x.Identifier).NotEmpty().WithMessage("Row 'Identifier' cannot be empty.");

            RuleFor(x => x.ColumnDefinitions).NotNull().WithMessage("'ColumnDefinitions' cannot be null");
            RuleFor(x => x.ColumnDefinitions).Must(x => x.Any()).WithMessage("'ColumnDefinitions' cannot be empty. It must contains at least one column definition.");

            RuleFor(x => x.ColumnDefinitions).SetCollectionValidator(new ColumnDefinitionValidator());
        }
    }
}
