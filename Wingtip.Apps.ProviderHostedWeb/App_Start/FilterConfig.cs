using System.Web;
using System.Web.Mvc;

namespace Wingtip.Apps.ProviderHostedWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
