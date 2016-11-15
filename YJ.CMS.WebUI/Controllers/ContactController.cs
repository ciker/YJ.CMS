using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.IBLL;

namespace YJ.CMS.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private IContentsService contentServicee;
        public ContactController(IContentsService contentServicee)
        {
            this.contentServicee = contentServicee;
        }
        //
        // GET: /Contact/

        public ActionResult Index()
        {
            var model = contentServicee.Query(c => c.cnt_id == 10).FirstOrDefault();
            if (model != null)
                ViewData.Model = model;

            return View();
        }

    }
}
