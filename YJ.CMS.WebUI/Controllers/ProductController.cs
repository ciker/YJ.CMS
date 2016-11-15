using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.IBLL;

namespace YJ.CMS.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        //
        // GET: /Product/

        public ActionResult Index()
        {
            //1.0 从url获取相关参数
            string pageindex = Request.QueryString["pageindex"];
            string pagesize = Request.QueryString["pagesize"];

            //2.0 开始初始化页码和每页显示的条数
            int ipageindex;
            int ipagesize;

            if (!int.TryParse(pageindex, out ipageindex))
            {
                ipageindex = 1;
            }
            if (!int.TryParse(pagesize, out ipagesize))
            {
                ipagesize = 1;
            }
            //3.0 计算出当前分页应该跳过的数据行数
            int skipCount = (ipageindex - 1) * ipagesize;
            //获取当前表数据的总条数
            ViewBag.TotalCount = productService.DbSet.Count();
            ViewBag.CurrentPage = ipageindex;

            var list = productService.DbSet.Where(n => n.is_lock == (int)Model.Enums.loginState.正常)
                .OrderByDescending(n => n.add_time).Skip(skipCount).Take(ipagesize).ToList();

            return View(list);
        }

        /// <summary>
        /// 负责根据产品类别来获取对应的产品数据，返回给index.cshtml视图 
        /// </summary>
        /// <param name="id">产品类别id</param>        
        public ActionResult productList(int id)
        {
            //TODO:请按照Index()方法自行实现本方法中的分页逻辑
            
            int noramal = (int)Model.Enums.loginState.正常;

            var list = productService.Query(c => c.p_id == id && c.is_lock == noramal).ToList();

            return View("Index", list);
        }

        /// <summary>
        /// 产品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detils(int id)
        {
            var model = productService.DbSet.Find(id);
            if (model != null)
                ViewData.Model = model;

            return View();            
        }

    }
}
