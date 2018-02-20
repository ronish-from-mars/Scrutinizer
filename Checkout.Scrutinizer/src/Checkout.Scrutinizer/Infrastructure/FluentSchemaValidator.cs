namespace Checkout.Scrutinizer.Infrastructure
{
    using System.Linq;

    using FluentValidation;

    using Checkout.Scrutinizer.Core;

    public class FluentSchemaValidator: AbstractValidator<RawFileSchema>, IFluentSchemaValidator
    {
        public FluentSchemaValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Schema).NotNull().WithMessage("'Schema' cannot be null");

            //RuleFor(x => x.OutputFileName).NotNull().WithMessage("'OutputFileName' is not present.");
            //RuleFor(x => x.OutputFileName).NotEmpty().WithMessage("'OutputFileName' cannot be empty.");

            RuleFor(x => x.FileFormat).NotNull().NotEmpty().WithMessage("'FileFormat' cannot be blank.");

            //RuleFor(x => x.FileFormat).NotNull().Must(x => x.ToLower().Equals("fixed") || x.ToLower().Equals("separated")).WithMessage("'FileFormat' can either be 'fixed' or 'separated'");
            //RuleFor(x => x.FileFormat).NotEmpty().Must(x => x.ToLower().Equals("fixed") || x.ToLower().Equals("separated")).WithMessage("'FileFormat' can either be 'fixed' or 'separated'");

            RuleFor(x => x.Delimeter).NotNull();
            RuleFor(x => x.Delimeter).NotEmpty();

            RuleFor(x => x.Schema.RowDefinitions).NotNull().WithMessage("'RowDefinitions' cannot be null");
            RuleFor(x => x.Schema.RowDefinitions).NotNull().Must(x => x.Any()).WithMessage("'RowDefinitions' cannot be empty. It must contains at least one row definition.");

            RuleFor(x => x.Schema.RowDefinitions).SetCollectionValidator(new RowDefinitionValidator());

            RuleFor(x => x).Custom((fileSchema, context) =>
            {
                if (fileSchema.FileFormat != null)
                {
                    if (fileSchema.FileFormat.ToLower().Equals("fixed"))
                    {
                        foreach (var rowDefinition in fileSchema.Schema.RowDefinitions)
                        {
                            foreach (var columnDefinition in rowDefinition.ColumnDefinitions)
                            {
                                if (columnDefinition.Length == null)
                                {
                                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(columnDefinition), "Length cannot be null for a fixed length file", null));
                                }
                            }
                        }
                    }
                    else if (fileSchema.FileFormat.ToLower().Equals("separated"))
                    {
                        if (string.IsNullOrEmpty(fileSchema.Delimeter))
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure("Delimeter", "Delimeter should be present if file format is set to 'separated", null));
                        }
                    }
                    else
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure("File Format", "'FileFormat' can either be 'fixed' or 'separated'", null));
                    }
                }
            });
        }
    }
}
