using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.IBLL;
using YJ.CMS.Common;

namespace YJ.CMS.WebUI.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private IUserInfoService userInfoService;

        public LoginController(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }
        //
        // GET: /Admin/Login/

        public ActionResult Index()
        {                        
            return View();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>        
        public ActionResult Vcode()
        {
            string vCode = ValidataCode.CreateRandomCode(4);
            Session["vCode"] = vCode;
            byte[] buffer = ValidataCode.DrawImage(vCode, 20, background: System.Drawing.Color.White);
            return File(buffer, "image/gif");
        }

        [HttpPost]
        public ActionResult Login(Model.ViewModel.UserInfoView model)
        {
            ////由于和新增用户的功能有冲突，所以此处只验证当前登录界面传入的属性合法性
            //bool uname = ModelState.IsValidField("u_name");
            //bool pwd = ModelState.IsValidField("u_pwd");
            //bool vcode = ModelState.IsValidField("VCode");
            try
            {
                #region 非空校验
                if (string.IsNullOrEmpty(model.u_name) || string.IsNullOrEmpty(model.u_pwd))
                {
                    ModelState.AddModelError("", "请输入用户名和密码");
                }
                if (Session["vCode"] == null)
                {
                    ModelState.AddModelError("", "请输入验证码");                    
                }
               
                #endregion
                if (ModelState.IsValid)
                {
                    if (Session["vCode"] != null && !Session["vCode"].ToString().Equals(model.VCode, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModelState.AddModelError("", "验证码输入有误");
                        Session["vCode"] = "";
                        return View("Index");
                    }
                    var userinfo = userInfoService.Login(model.u_name, CommonHelper.MD5Entry(model.u_pwd));
                    if (userinfo == null)
                    {
                        ModelState.AddModelError("", "用户名和密码不正确");
                        return View("Index");
                    }

                    Session["userInfo"] = userinfo;
                    TempData["currentUser"] = userinfo.real_name;

                    return Redirect("/Admin/Category/Index");
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult Cancle()
        {
            if (Session["userInfo"] != null)
                Session["userInfo"] = null;
            return Content("ok");
        }

    }
}
