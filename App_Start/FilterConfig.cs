using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;

namespace FYP_WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
