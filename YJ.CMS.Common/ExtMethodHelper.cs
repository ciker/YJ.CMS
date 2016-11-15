using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YJ.CMS.Common
{
    public static class ExtMethodHelper
    {
        /// <summary>
        /// 将日期格式化成年-月-日字符串返回
        /// </summary>       
        public static string DateTimeFmtYYYYMMDD(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// /// 将日期格式化成  时：分：秒 字符串返回
        /// </summary>     
        public static string DateTimeFmtHHMMSS(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
