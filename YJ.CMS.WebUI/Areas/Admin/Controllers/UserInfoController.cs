using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using YJ.CMS.IBLL;
using YJ.CMS.EntityMap;
using YJ.CMS.Model.ViewModel;

namespace YJ.CMS.WebUI.Areas.Admin.Controllers
{
    public class UserInfoController : Controller
    {
        private IUserInfoService userinfoService;
        public UserInfoController(IUserInfoService userinfoService)
        {
            this.userinfoService = userinfoService;
        }
        //
        // GET: /Admin/UserInfo/

        public ActionResult Index()
        {
            var list = userinfoService.Query(u => u.u_is_lock == (int)Model.Enums.loginState.正常).ToList();
            if (list.Count > 0)
                ViewData.Model = list.Select(u => u.EntityMap());   //要先转化成集合，才能映射..

            return View();
        }

        //
        // GET: /Admin/UserInfo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/UserInfo/Create

        [HttpPost]
        public ActionResult Create(UserInfoView model)
        {
            try
            {
                model.u_is_lock = (int)Model.Enums.loginState.正常;
                model.u_add_time = DateTime.Now;

                //bool name = ModelState.IsValidField("u_name");
                //bool realname = ModelState.IsValidField("real_name"); //指定要验证的属性名..
                if (ModelState.IsValid)
                {
                    model.u_pwd = Common.CommonHelper.MD5Entry(model.u_pwd);
                    userinfoService.Add(model.EntityMap());
                    userinfoService.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// 校验用户名是否存在
        /// </summary>        
        public string CheckUname()
        {
            //注意：QueryString 中的key的名称应该取自UserInfoView 实体中的u_name 属性
            string uname = Request.QueryString["u_name"];

            //这里利用Any()来匹配 如果存在则返回true，但是此处应该取反以后返回
            var res = !userinfoService.DbSet.Any(c => c.u_name == uname);

            //注意点：一定是返回小写的true或者false true:表示可以使用..
            return res.ToString().ToLower();
        }

        //
        // GET: /Admin/UserInfo/Edit/5

        public ActionResult Edit(int id)
        {
            ViewData.Model = userinfoService.DbSet.Find(id).EntityMap();
            return View();
        }

        //
        // POST: /Admin/UserInfo/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, UserInfoView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.user_id = id;
                    if (userinfoService.Update(model.EntityMap(), new string[] { "u_name", "u_pwd", "real_name", "u_telephone", "u_email" }))
                    {
                        userinfoService.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }       

        //
        // POST: /Admin/UserInfo/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var model = userinfoService.DbSet.Find(id);
                if (model != null)
                {
                    userinfoService.Remove(model, true);
                    return Content("ok");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// 禁用用户
        /// </summary>   
        public ActionResult Stop(int id)
        {
            var userinfo = userinfoService.Query(c => c.user_id == id).FirstOrDefault();
            if (userinfo != null)
            {
                userinfo.u_is_lock = (int)Model.Enums.loginState.冻结;
                userinfoService.SaveChanges();
            }

            return Json(new { status = "sucess", msg = "ok" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 启用用户
        /// </summary>                
        public ActionResult Start(int id)
        {
            var userinfo = userinfoService.Query(c => c.user_id == id).FirstOrDefault();
            if (userinfo != null)
            {
                userinfo.u_is_lock = (int)Model.Enums.loginState.正常;
                userinfoService.SaveChanges();
            }
          
            return Json(new { status = "sucess", msg = "ok" }, JsonRequestBehavior.AllowGet);
        } 

    }
}
