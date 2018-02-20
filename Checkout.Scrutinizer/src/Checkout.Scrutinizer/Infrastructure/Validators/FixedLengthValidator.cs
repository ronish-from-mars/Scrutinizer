namespace Checkout.Scrutinizer.Infrastructure.Validators
{
    using FluentValidation;
    using System.Collections.Generic;

    public class FixedLengthValidator : AbstractValidator<Core.ValidationResult>
    {
        public FixedLengthValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(c => c).SetValidator(new AlignmentValidator());
            RuleFor(c => c).SetValidator(new AllowedValuesValidator());
        }
    }
}
