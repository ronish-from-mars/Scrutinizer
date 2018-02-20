namespace Checkout.Scrutinizer.UI.Utilities
{
    using System.Threading.Tasks;

    public interface IViewRenderingService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
