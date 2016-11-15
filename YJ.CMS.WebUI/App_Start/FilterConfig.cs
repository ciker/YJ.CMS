using System.Web;
using System.Web.Mvc;
using YJ.CMS.WebUI.Filter;

namespace YJ.CMS.WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionFilterAttribute());    //注册异常过滤.
        }
    }
}