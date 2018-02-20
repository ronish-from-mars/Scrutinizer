namespace Checkout.Scrutinizer.Core
{
    public interface ISchemaBuilder<T> where T : FileSchemaBase
    {
        T BuildSchema(RawFileSchema rawSchema);
    }
}
