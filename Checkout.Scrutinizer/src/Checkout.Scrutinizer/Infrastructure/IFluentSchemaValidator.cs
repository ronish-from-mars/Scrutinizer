using Checkout.Scrutinizer.Core;
using FluentValidation;

namespace Checkout.Scrutinizer.Infrastructure
{
    public interface IFluentSchemaValidator: IValidator<RawFileSchema>
    {
    }
}
