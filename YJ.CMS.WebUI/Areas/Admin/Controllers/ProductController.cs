using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YJ.CMS.IBLL;
using YJ.CMS.EntityMap;
using YJ.CMS.Model.ViewModel;
using System.IO;
using YJ.CMS.Common;

using System.Linq.Expressions;

namespace YJ.CMS.WebUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private IProductService productService;
        private ICategoryService categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }
        //
        // GET: /Admin/Product/

        public ActionResult Index()
        {
            var clist = categoryService.DbSet.ToList();
            clist.Insert(0, new Model.Category() { c_id = -1, c_title = "---请选择---" });

            ViewData["cateid"] = new SelectList(clist, "c_id", "c_title");

            var normal = (int)Model.Enums.loginState.正常;
            var list = productService.WhereJoin(c => c.is_lock == normal, new string[] { "Category" })
                .Select(c => c.EntityMap());

            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string pname = form["pname"];
            string cateid = form["cateid"];

            int icateid;
            if (int.TryParse(cateid, out icateid) == false)
            {
                icateid = -1;
            }

            #region 2.0 **** 根据条件利用表达式树来动态生成lambda表达式 ****

            //1.0 先定义最终要组合的lambda 表达式，默认赋值为true
            Expression cod = null;// Expression.Constant(true);  // true

            //2.0 定义参数类型
            ParameterExpression p1 = Expression.Parameter(typeof(Model.Product), "c");

            //3.0 如果标题不为空,则在cod 后面添加一个 c.p_title.Contains(pname)
            if (!string.IsNullOrEmpty(pname))
            {
                // c=>c.p_title.Contains(pname)
                //3.0.1 定义常量表达式
                ConstantExpression cpname = Expression.Constant(pname);
                //3.0.2 构造lambda表达式  ,query1:c.p_title.Contains(pname)
                var query1 = Expression.Call(Expression.PropertyOrField(p1, "p_title"),
                    typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                    cpname);

                //3.0.3 将query1 追加到cod 中
                if (cod == null)
                {
                    cod = query1;
                }
                else
                {
                    cod = Expression.Or(query1, cod);  //c.p_title.Contains(pname)
                }
            }

            //4.0 如果类别id》0 则应该 利用 and()方法将其lambda表达式追加到cod
            if (icateid > 0)
            {
                //4.0.1 定义常量表达式ccid
                ConstantExpression ccid = Expression.Constant(icateid);
                //4.0.2 从参数表达式中获取其属性category_id 作为一个成员表达式输出
                MemberExpression memberCateId = Expression.PropertyOrField(p1, "category_id");
                //4.0.3 利用Expression.Equal 进行lambda表达式的组合 
                var query2 = Expression.Equal(memberCateId, ccid);  //c.category_id == icateid
                //3.0.3 将query2 追加到cod 中
                if (cod == null)
                {
                    cod = query2;
                }
                else
                {
                    cod = Expression.Or(query2, cod);
                }
            }

            //5.0 将Expression 类型变成Expression<Func<Product,bool>>    c.category_id == icateid || c.p_title.Contains(pname)
            Expression<Func<Model.Product, bool>> restLambda = Expression.Lambda<Func<Model.Product, bool>>(cod, p1);

            var list = productService.Query(restLambda).Select(c => c.EntityMap());

            #endregion

            SetClist();

            return View(list);
        }

        #region 新增
        /// <summary>
        /// 新建
        /// </summary>        
        public ActionResult Create()
        {
            SetClist();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ProductView model)
        {
            model.sort_id = 1;
            model.click = 0;
            model.is_lock = 0;
            model.add_time = DateTime.Now;
            try
            {
                productService.Add(model.EntityMap());
                productService.SaveChanges();

                return Redirect("/Admin/Product/Index");
            }
            catch (Exception ex)
            {
                SetClist();
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult Edit(int id)
        {
            SetClist();
            var model = productService.Query(c => c.p_id == id).FirstOrDefault();
            if (model != null)
                return View(model.EntityMap());

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, ProductView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = productService.Query(c => c.p_id == id).FirstOrDefault();
                    entity.p_title = model.p_title;
                    entity.p_content = model.p_content;
                    entity.p_photo_no = model.p_photo_no;
                    entity.category_id = model.category_id;

                    productService.SaveChanges();

                    return Redirect("/Admin/Product/Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                SetClist();
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>                
        public ActionResult Delete(int id)
        {
            var model = productService.Query(c => c.p_id == id).SingleOrDefault();
            productService.Remove(model, true);
            productService.SaveChanges();
            return View();
        }
        #endregion

        #region 上传
        /// <summary>
        /// 上传图片
        /// </summary>        
        public ActionResult Upload(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(int id, HttpPostedFileBase uploadFile)
        {
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                var fileEx = Path.GetExtension(uploadFile.FileName);
                if (fileEx == ".jpg" || fileEx == ".gif" || fileEx == ".png" || fileEx == ".jpeg")
                {
                    //var fileName = Path.GetFileNameWithoutExtension(uploadFile.FileName); 文件重命名.
                    string path = "/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                    Directory.CreateDirectory(Path.GetDirectoryName(Server.MapPath(path)));
                    string newFileName = CommonHelper.GetMD5(uploadFile.InputStream) + fileEx;
                    uploadFile.SaveAs(Server.MapPath(path) + newFileName);

                    //2.0.4 将文件名更新到数据库表字段中 img_url
                    var model = productService.Query(c => c.p_id == id).FirstOrDefault();
                    model.img_url = newFileName;
                    productService.SaveChanges();
                    RedirectToAction("Index", "Product");
                }
            }
            return View();
        }
        #endregion

        /// <summary>
        /// 设置下拉列表
        /// </summary>
        private void SetClist()
        {
            var clist = categoryService.DbSet.ToList();
            clist.Insert(0, new Model.Category() { c_id = -1, c_title = "---请选择---" });
            ViewBag.Clist = new SelectList(clist, "c_id", "c_title");
        }

    }
}
