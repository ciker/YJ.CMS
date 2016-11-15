using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.IBLL;
using YJ.CMS.EntityMap;

namespace YJ.CMS.WebUI.Controllers
{
    public class FeedBackController : Controller
    {
        private IfeedbackService feedback;
        public FeedBackController(IfeedbackService feedback)
        {
            this.feedback = feedback;
        }
        //
        // GET: /FeedBack/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 客户留言提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(Model.ViewModel.feedbackView model)
        {
            model.fuser_qq = "";
            model.fuser_email = "";
            model.fadd_time = DateTime.Now;
            model.freply_content = "";
            model.freply_time = DateTime.Now;
            model.is_lock = false;
            try
            {

                if (ModelState.IsValid == false)
                {
                    return View();
                }

                //开始正常处理逻辑（相当于做新增操作 ）
                feedback.Add(model.EntityMap());
                feedback.SaveChanges();

                return Content("<script>alert('恭喜，您的留言我们将会尽快回复');window.location=window.location;</script>");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

    }
}
