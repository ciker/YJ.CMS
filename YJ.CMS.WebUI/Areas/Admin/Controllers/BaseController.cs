using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.IBLL;

namespace YJ.CMS.WebUI.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected ICategoryService  categoryBLL;
        //protected IContentsBLL contentsBLL;
        //protected IfeedbackBLL feedbackBLL;
        //protected IMenusBLL menusBLL;
        //protected INewsBLL newsBLL;
        //protected IProductBLL productBLL;
        protected IUserInfoService userInfoBLL;
        protected Model.UserInfo userInfo;

        /// <summary>
        /// 登陆校验
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Session["userInfo"] == null)
            {
                //TODO 写入日志
                filterContext.HttpContext.Response.Redirect("/Admin/Login/index");
                Response.End();                
            }
            userInfo = Session["userInfo"] as Model.UserInfo;

        }
    }
}
