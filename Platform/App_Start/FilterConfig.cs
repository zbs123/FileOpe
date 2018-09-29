
using Platform.Commonn;
using System.Web;
using System.Web.Mvc;

namespace Platform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new UserAttribute());
            //filters.Add(new AuthenticationAttribute());
            filters.Add(new MyExceptionFileAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
