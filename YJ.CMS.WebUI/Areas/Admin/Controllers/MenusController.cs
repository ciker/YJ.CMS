using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.EntityMap;

namespace YJ.CMS.WebUI.Areas.Admin.Controllers
{    
    public class MenusController : Controller //BaseController
    {
        private IBLL.IMenusService menuService;
        public MenusController(IBLL.IMenusService menuService)
        {
            this.menuService = menuService;
        }

        //
        // GET: /Admin/Menus/

        public ActionResult Index()
        {
            //对于集合 要转换成集合投影
            ViewData.Model = menuService.Query(c => c.m_status == (int)Model.Enums.loginState.正常).ToList()
                .Select(c => c.EntityMap());

            return View();  //return View(list);
        }

        /// <summary>
        /// 左侧菜单视图
        /// </summary> 
        [ChildActionOnly]
        public ActionResult GetMenus()
        {
            var menusList = menuService.DbSet.Where(c => c.m_status == (int)Model.Enums.loginState.正常)                
                .OrderBy(c => c.m_sortid).ToList();

            return PartialView(menusList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Model.ViewModel.MenusView model)
        {
            model.m_status = (int)Model.Enums.loginState.正常;
            model.m_add_time = DateTime.Now;

            menuService.Add(model.EntityMap());
            if (menuService.SaveChanges() > 0)
            {
                return RedirectToAction("Index", "Menus");
            }
            return View();
        }

    }
}
