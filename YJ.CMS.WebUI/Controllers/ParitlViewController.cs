using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.IBLL;

namespace YJ.CMS.WebUI.Controllers
{
    public class ParitlViewController : Controller
    {
        private ICategoryService categoryService;
        private IContentsService contentService;

        public ParitlViewController(ICategoryService categoryService, IContentsService contentService)
        {
            this.categoryService = categoryService;
            this.contentService = contentService;
        }
        //
        // GET: /ParitlView/
      
        /// <summary>
        /// 基础类别固定为cateid= 2 ，产品分部视图
        /// </summary>        
        public ActionResult ProductCateList()
        {
            var clist = categoryService.Query(c => c.c_type == 2).ToList();

            return PartialView(clist);
        }


        /// <summary>
        /// 联系我们，基础数据类别id固定为7
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactPv()
        {
            var model = contentService.Query(c => c.category_id == 7).OrderByDescending(c => c.cnt_add_time).FirstOrDefault();

            return PartialView(model);
        }

        /// <summary>
        /// 公司简介
        /// </summary>
        /// <returns></returns>
        public ActionResult ComIntroduce()
        {
            ViewData.Model = contentService.Query(c => c.category_id == 6);
            
            return View();
        }

    }
}
