namespace Checkout.Scrutinizer.Core
{
    public interface IDataSourceParser
    {
        DataSource ParseDataSource(string jsonData);
    }
}
