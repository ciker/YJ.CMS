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
    public class NewsController : Controller
    {
        private INewsService newsService;
        private ICategoryService categoryService;

        public NewsController(INewsService newsService, ICategoryService categoryService)
        {
            this.newsService = newsService;
            this.categoryService = categoryService;
        }
        //
        // GET: /Admin/News/

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
            ViewBag.TotalCount = newsService.DbSet.Count();
            ViewBag.CurrentPage = ipageindex;

            var list = newsService.DbSet.Where(n => n.n_is_lock == (int)Model.Enums.loginState.正常)
                .OrderByDescending(n => n.add_time).Skip(skipCount).Take(ipagesize).ToList();
            ViewData.Model = list.Select(s => s.EntityMap());
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 添加新闻
        /// </summary>
        [HttpPost]
        public ActionResult Create(NewsView model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View();
                }

                model.n_is_lock = (int)Model.Enums.loginState.正常;
                model.add_time = DateTime.Now;

                newsService.Add(model.EntityMap());
                newsService.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// 编辑新闻
        /// </summary>                
        public ActionResult Edit(int id)
        {
            SetClist();

            //ViewBag.clist=
            var model = newsService.DbSet.Find(id);
            if (model != null)
                ViewData.Model = model.EntityMap();

            return View();
        }

        /// <summary>
        /// 下拉列表
        /// </summary>
        private void SetClist()
        {
            var cList = categoryService.DbSet.ToList();
            SelectList list = new SelectList(cList, "c_id", "c_title");
            ViewBag.clist = list;
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, NewsView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region 1更新
                    //if (newsService.Update(model.EntityMap(), new string[] { "category_id", "n_title", "n_author", "n_form", "n_content" }))
                    //{
                    //    newsService.SaveChanges();
                    //    return RedirectToAction("Index");
                    //}
                    #endregion
                    var entity = newsService.DbSet.Find(id);
                    if (entity != null)
                    {
                        entity.category_id = model.category_id;
                        entity.n_title = model.n_title;
                        entity.n_author = model.n_author;
                        entity.n_form = model.n_form;
                        entity.n_content = model.n_content;
                    }
                    newsService.SaveChanges();
                    return Redirect("/Admin/News/Index");
                }                
                return View();
            }
            catch (Exception ex)
            {
                SetClist();
                ModelState.AddModelError("error", ex.Message);
                return View();
            }
        }



    }
}
