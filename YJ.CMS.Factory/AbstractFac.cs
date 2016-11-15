using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YJ.CMS.IDAL;

namespace YJ.CMS.Factory
{
    public class AbstractFac
    {
        #region none
        private static readonly string assembly = System.Configuration.ConfigurationManager.AppSettings["assemblyName"];
        private static readonly string space = System.Configuration.ConfigurationManager.AppSettings["spaceName"];

        /// <summary>
        /// 反射创建实例 一般鸟
        /// </summary>
        /// <returns></returns>
        public static IUserInfoDAL GetUserInfoInstance()
        {
            object obj = System.Reflection.Assembly.Load(assembly).CreateInstance(space + ".UserInfoDAL");

            return obj as IUserInfoDAL;
        }


        public static ICategoryDAL GetCategoryInstance()
        {
            object obj = System.Reflection.Assembly.Load(assembly).CreateInstance(space + ".CategoryDAL");

            return obj as ICategoryDAL;
        }
        #endregion        

    }
}
