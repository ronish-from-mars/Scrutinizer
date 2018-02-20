namespace Checkout.Scrutinizer.Infrastructure.Extensions
{
    using Newtonsoft.Json;

    public static class StringExtensions
    {
        public static T Deserialize<T>(this string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
