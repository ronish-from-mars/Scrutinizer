namespace Checkout.Scrutinizer.Infrastructure
{
    using System.Collections.Generic;

    using Checkout.Scrutinizer.Core;

    public class SchemaValidator : ISchemaValidator
    {
        private readonly IFluentSchemaValidator _fluentSchemaValidator;

        public SchemaValidator(IFluentSchemaValidator fluentSchemaValidator)
        {
            _fluentSchemaValidator = fluentSchemaValidator;
        }

        public IEnumerable<ValidationResult> ValidateSchema(RawFileSchema fileSchema)
        {
            var validationFailureResults = new List<ValidationResult>();
            var validationResult = _fluentSchemaValidator.Validate(fileSchema);

            if (!validationResult.IsValid)
            {
                foreach (var validationError in validationResult.Errors)
                {
                    validationFailureResults.Add(new ValidationResult
                    {
                        ColumnName = validationError.PropertyName,
                        ErrorMessage = validationError.ErrorMessage,
                        AttemptedValue = validationError.AttemptedValue.ToString()
                    });
                }
            }

            return validationFailureResults;
        }
    }
}
