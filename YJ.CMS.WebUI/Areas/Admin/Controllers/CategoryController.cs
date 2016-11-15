using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.EntityMap;
using YJ.CMS.Model.ViewModel;

namespace YJ.CMS.WebUI.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private IBLL.ICategoryService categoryService;

        public CategoryController(IBLL.ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        //
        // GET: /Admin/Category/

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
            ViewBag.TotalCount = categoryService.DbSet.Count();
            ViewBag.CurrentPage = ipageindex;
            //4.0 实现分页逻辑代码
            var list = categoryService.DbSet.OrderByDescending(c => c.c_id).Skip(skipCount).Take(ipagesize).ToList();
            var listModelView = list.Select(c => c.EntityMap());

            return View(listModelView);

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CategoryView model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View();
                }

                //1.0 利用EF做新增操作
                categoryService.Add(model.EntityMap());
                categoryService.SaveChanges();

                return Redirect("/Admin/Category/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult Edit(int id)
        {
            //根据id从数据库中查询出分类数据实体Category，再通过EntityMap()方法转换成CategoryView实体
            var model = categoryService.Query(c => c.c_id == id).FirstOrDefault();
            if (model != null)
                ViewData.Model = model.EntityMap();

            //将CategoryView实体 传入视图Edit.cshtml中
            return View();
        }

        [HttpPost]
        public ActionResult Edit(CategoryView model, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.c_id = id;
                    if (categoryService.Update(model.EntityMap(), new string[] { "c_type", "c_title" }))//更新属性
                    {
                        categoryService.SaveChanges();
                        return Redirect("/Admin/Category/Index");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

    }
}
