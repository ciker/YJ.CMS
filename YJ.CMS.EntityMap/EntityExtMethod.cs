using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YJ.CMS.Model;
using YJ.CMS.Model.ViewModel;

namespace YJ.CMS.EntityMap
{
    /// <summary>
    /// 负责存放实体层中视图实体和EF实体相互转换的扩展方法
    /// </summary>
    public static class EntityExtMethod
    {
        #region 1.0  负责将当前项目中所有的实体进行一次映射，将来在其他扩展方法中可以直接重用
        /// <summary>
        /// 负责将当前项目中所有的实体进行一次映射
        /// </summary>
        private static void ModelToModelView()
        {
            Mapper.CreateMap<Category, CategoryView>();
            Mapper.CreateMap<Contents, ContentsView>();
            Mapper.CreateMap<feedback, feedbackView>();
            Mapper.CreateMap<Menus, MenusView>();
            Mapper.CreateMap<News, NewsView>();
            Mapper.CreateMap<Product, ProductView>();
            Mapper.CreateMap<UserInfo, UserInfoView>();
        }

        private static void ModelViewToModel()
        {
            //1.0 先创建Category 和CategoryView的映射关系
            Mapper.CreateMap<CategoryView, Category>();
            //2.0 映射当前Category实体中的所有导航属性
            Mapper.CreateMap<ContentsView, Contents>();
            Mapper.CreateMap<feedbackView, feedback>();
            Mapper.CreateMap<MenusView, Menus>();
            Mapper.CreateMap<NewsView, News>();
            Mapper.CreateMap<ProductView, Product>();
            Mapper.CreateMap<UserInfoView, UserInfo>();
        }

        #endregion

        #region 2.0 Category表相关映射
        public static CategoryView EntityMap(this Category model)
        {
            //利用AutoMapper 来将Category 的属性值动态拷贝到CategoryView类中对应的同名属性值中
            ModelToModelView();
            //3.0 开始转换
            return Mapper.Map<Category, CategoryView>(model);
        }

        public static Category EntityMap(this CategoryView model)
        {
            //利用AutoMapper 来将Category 的属性值动态拷贝到CategoryView类中对应的同名属性值中
            ModelViewToModel();

            //3.0 开始转换
            return Mapper.Map<CategoryView, Category>(model);
        }
        #endregion

        #region 3.0 ContentsView 表相关映射
        public static ContentsView EntityMap(this Contents model)
        {
            ModelToModelView();
            return Mapper.Map<Contents, ContentsView>(model);
        }

        public static Contents EntityMap(this ContentsView model)
        {
            ModelViewToModel();
            return Mapper.Map<ContentsView, Contents>(model);
        }

        #endregion

        #region 4.0

        public static feedbackView EntityMap(this feedback model)
        {
            ModelToModelView();
            return Mapper.Map<feedback, feedbackView>(model);
        }

        public static feedback EntityMap(this feedbackView model)
        {
            ModelViewToModel();
            return Mapper.Map<feedbackView, feedback>(model);
        }

        #endregion

        #region 5.0

        public static MenusView EntityMap(this Menus model)
        {
            ModelToModelView();
            return Mapper.Map<Menus, MenusView>(model);
        }

        public static Menus EntityMap(this MenusView model)
        {
            ModelViewToModel();
            return Mapper.Map<MenusView, Menus>(model);
        }

        #endregion

        #region 6.0

        public static NewsView EntityMap(this News model)
        {
            ModelToModelView();
            return Mapper.Map<News, NewsView>(model);
        }

        public static News EntityMap(this NewsView model)
        {
            ModelViewToModel();
            return Mapper.Map<NewsView, News>(model);
        }

        #endregion

        #region 7.0

        public static ProductView EntityMap(this Product model)
        {
            ModelToModelView();
            return Mapper.Map<Product, ProductView>(model);
        }

        public static Product EntityMap(this ProductView model)
        {
            ModelViewToModel();
            return Mapper.Map<ProductView, Product>(model);
        }

        #endregion

        #region 8.0

        public static UserInfoView EntityMap(this UserInfo model)
        {
            ModelToModelView();
            return Mapper.Map<UserInfo, UserInfoView>(model);
        }

        public static UserInfo EntityMap(this UserInfoView model)
        {
            ModelViewToModel();
            return Mapper.Map<UserInfoView, UserInfo>(model);
        }

        #endregion


    }
}
