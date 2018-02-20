namespace Checkout.Scrutinizer.Infrastructure
{
    using Checkout.Scrutinizer.Core;
    using Checkout.Scrutinizer.Infrastructure.Extensions;

    public class DataSourceParser : IDataSourceParser
    {
        public DataSource ParseDataSource(string jsonData)
        {
            return jsonData.Deserialize<DataSource>();
        }
    }
}
