using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace YJ.CMS.EFDAL
{  
    public class BaseDBContext
    {
        /// <summary>
        /// 获取EF上下文实例唯一
        /// </summary>
        /// <returns></returns>
        public static DbContext GetDBContext()
        {
            var db = CallContext.GetData("dbContext") as DbContext;
            if (db == null)
            {
                db = new Model1Container();
                CallContext.SetData("dbContext", db);
            }

            return db;
        }
               
    }
}
