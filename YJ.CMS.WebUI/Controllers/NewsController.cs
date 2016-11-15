using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.IBLL;

namespace YJ.CMS.WebUI.Controllers
{
    public class NewsController : Controller
    {
        private INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }
        //
        // GET: /News/

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
                ipagesize = 4;
            }
            //3.0 计算出当前分页应该跳过的数据行数
            int skipCount = (ipageindex - 1) * ipagesize;
            //获取当前表数据的总条数
            ViewBag.TotalCount = newsService.DbSet.Count();
            ViewBag.CurrentPage = ipageindex;

            var list = newsService.DbSet.Where(n => n.n_is_lock == (int)Model.Enums.loginState.正常)
                .OrderByDescending(n => n.add_time).Skip(skipCount).Take(ipagesize).ToList();

            return View(list);            
        }

        /// <summary>
        /// 负责呈现某个新闻的详细
        /// </summary>
        /// <returns></returns>
        public ActionResult Detils(int id)
        {
            var model = newsService.Query(c => c.n_id == id).FirstOrDefault();

            return View(model);
        }

        /// <summary>
        /// 负责查询公司新闻
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyNews()
        {
            //由于数据库的基础数据中公司新闻的id =1 ，所以在此处可以直接筛选出 cateid=1的所有新闻数据即可
            var newlist = newsService.Query(c => c.n_is_lock == (int)Model.Enums.loginState.正常
                && c.category_id == 1).ToList();
            return View("Index", newlist);
        }

        /// <summary>
        /// 负责查询行业新闻
        /// </summary>
        /// <returns></returns>
        public ActionResult HangYeNews()
        {
            //由于数据库的基础数据中行业新闻的id =2 ，所以在此处可以直接筛选出 cateid=1的所有新闻数据即可           
            var newlist = (from c in newsService.DbSet
                           where c.n_is_lock == (int)Model.Enums.loginState.正常
                           && c.category_id == 2
                           select c).ToList();

            //var newlist = (from c in base.newsBLL.DbSet
            //               where c.n_is_lock == (int)Enums.EStatus.Normal
            //               where c.category_id == 2
            //               select c).ToList();

            return View("Index", newlist);
        }

    }
}
