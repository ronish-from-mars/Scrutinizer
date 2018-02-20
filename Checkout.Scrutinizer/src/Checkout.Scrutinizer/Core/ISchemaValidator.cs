namespace Checkout.Scrutinizer.Core
{
    using System.Collections.Generic;

    public interface ISchemaValidator
    {
        IEnumerable<ValidationResult> ValidateSchema(RawFileSchema fileSchema);
    }
}
