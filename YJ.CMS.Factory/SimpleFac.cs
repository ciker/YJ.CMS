using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YJ.CMS.Factory
{
    public class SimpleFac
    {
        /// <summary>
        /// 获取EF实例 菜鸟直接new
        /// </summary>
        public static IDAL.IUserInfoDAL GetUserInfoInstance()
        {
            return new EFDAL.UserInfoDAL();
        }

        public static IDAL.ICategoryDAL GetCategoryInstance()
        {
            return new EFDAL.CategoryDAL();
        }
    }
}
