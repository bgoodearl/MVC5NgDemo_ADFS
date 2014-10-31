using System.Web;
using System.Web.Mvc;
using MVCDemo.Filters;

namespace MVCDemo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());

            //NOTE: Comment this out to allow Visual Studio interactions with site being debugged)
            filters.Add(new ContentSecurityPolicyFilterAttribute());
        }
    }
}
