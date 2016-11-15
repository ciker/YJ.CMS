using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YJ.CMS.IBLL
{
    public partial interface IUserInfoService
    {
        /// <summary>
        /// 查询用户是否存在
        /// </summary>
        Model.UserInfo Login(string uname, string md5pwd);
    }
}
