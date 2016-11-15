using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YJ.CMS.WebUI.Filter
{
    public class ExceptionFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            //记录日志
            string url = filterContext.HttpContext.Request.RawUrl;
            string ip = filterContext.HttpContext.Request.UserHostAddress;
            Common.LogHelper.WriteLog(string.Format("IP为{0}的用户，请求地址{1}时发生错误，错误信息：{2}", ip, url,
                filterContext.Exception.ToString()));
            //跳转
            filterContext.HttpContext.Response.Redirect("/Views/Shared/Error.cshtml");
        }

    }
}