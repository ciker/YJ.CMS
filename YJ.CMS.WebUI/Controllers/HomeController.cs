using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YJ.CMS.WebUI.Controllers
{
    public class HomeController : Controller
    {
        protected IBLL.IUserInfoService userInfoService;
        //
        // GET: /Home/
        public HomeController(IBLL.IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        public ActionResult Index()
        {
            //userInfoService = new BLL.UserInfoService();

            int normal = (int)Model.Enums.loginState.正常;

            ViewData.Model = userInfoService.Query(u => u.u_is_lock == normal).FirstOrDefault();
            return View();
        }

    }
}
