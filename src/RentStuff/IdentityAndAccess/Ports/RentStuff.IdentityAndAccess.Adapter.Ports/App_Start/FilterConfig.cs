using System.Web;
using System.Web.Mvc;

namespace RentStuff.IdentityAndAccess.Adapter.Ports
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
