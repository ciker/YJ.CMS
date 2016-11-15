using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Mvc
{
    public static class PagingHelper
    {
        /// <summary>
        /// 分页扩展
        /// </summary>
        /// <returns></returns>
        public static HtmlString PageNavigate(this HtmlHelper html, int currentPage, int pageSize, int totalCount)
        {
            var redirectTo = HttpContext.Current.Request.Url.AbsolutePath;
            pageSize = pageSize <= 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var sb = new StringBuilder();
            if (totalPages > 1)
            {
                if (currentPage != 1)
                    sb.AppendFormat("<a class='pageLink' href='{0}?pageIndex=1&pageSize={1}'>首页</a> ", redirectTo, pageSize);
                if (currentPage > 1)
                {//处理上一页的连接
                    sb.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>上一页</a> ",
                        redirectTo, currentPage - 1, pageSize);
                }

                sb.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理                           
                            sb.AppendFormat("<a class='cpb' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a> ",
                                redirectTo, currentPage, pageSize, currentPage);
                        }
                        else
                        {//一般页处理
                            sb.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a> ",
                                redirectTo, currentPage + i - currint, pageSize, currentPage + i - currint);
                        }
                    }
                    sb.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    sb.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>下一页</a> ",
                        redirectTo, currentPage + 1, pageSize);
                }

                sb.Append(" ");
                if (currentPage != totalPages)
                {
                    sb.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>末页</a> ",
                        redirectTo, totalPages, pageSize);
                }
                sb.Append(" ");
            }
            sb.AppendFormat("<label>第{0}页 / 共{1}页</label>", currentPage, totalPages);//这个统计加不加都行

            return new HtmlString(sb.ToString());
        }

    }
}
