using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace YJ.CMS.Common
{
    public class LogHelper
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logTxt"></param>
        public static void WriteLog(string logTxt)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("log");
            log.Error(logTxt);
        }
    }
}
