using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YJ.CMS.BLL
{
    public partial class UserInfoService
    {

        //public override void SetCurrentDAL()
        //{
        //    base.CurrentDal = Factory.SimpleFac.GetUserInfoInstance();//new EFDAL.UserInfoDAL();
        //}

        /// <summary>
        /// 查询用户是否存在
        /// </summary>
        public Model.UserInfo Login(string uname, string md5pwd)
        {
            return base.Query(c => c.u_name == uname && c.u_pwd == md5pwd).FirstOrDefault();
        }

    }
}
