using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;

namespace YJ.CMS.WebUI.Config
{
    public class AutofacConfig
    {
        ///获取bll层命名空间
        private static readonly string BllSpace = System.Configuration.ConfigurationManager.AppSettings["BllSpace"];

        /// <summary>
        /// 注册autofac组件
        /// </summary>
        public static void Register()
        {
            var builder = new ContainerBuilder();

            #region 注册控制器

            // You can register controllers all at once using assembly scanning...
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //// ...or you can register individual controlllers manually.
            //builder.RegisterType<HomeController>().InstancePerRequest();
            #endregion

            builder.RegisterGeneric(typeof(EFDAL.BaseDAL<>)).As(typeof(IDAL.IBaseDAL<>)).InstancePerHttpRequest();

            //注册类型 TODO 考虑用T4生成..
            builder.RegisterType<BLL.UserInfoService>().As<IBLL.IUserInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<BLL.CategoryService>().As<IBLL.ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<BLL.MenusService>().As<IBLL.IMenusService>().InstancePerLifetimeScope();
            builder.RegisterType<BLL.NewsService>().As<IBLL.INewsService>().InstancePerLifetimeScope();
            builder.RegisterType<BLL.ProductService>().As<IBLL.IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<BLL.ContentsService>().As<IBLL.IContentsService>().InstancePerLifetimeScope();
            builder.RegisterType<BLL.feedbackService>().As<IBLL.IfeedbackService>().InstancePerLifetimeScope();

            ///与bll建立联系..
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load(BllSpace)).InstancePerHttpRequest();
            //可以筛选程序集下指定的类..
            //builder.RegisterTypes(System.Reflection.Assembly.Load(BllSpace).GetTypes()
            //      .Where(t => t.Name.StartsWith("d")).ToArray()).InstancePerHttpRequest();

            IContainer container = builder.Build();    //创建容器..
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));  //解析依赖关系..
        }

    }
}