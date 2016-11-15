using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YJ.CMS.IDAL
{
    public interface IBaseDAL<TEntity> where TEntity : class
    {
        DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <param name="predicate">主表TEntity 的查询条件</param>
        /// <param name="IncludeTables">与主表TEntity 要链接的表数组</param>
        /// <returns></returns>
        List<TEntity> WhereJoin(Expression<Func<TEntity, bool>> predicate, string[] IncludeTables);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TKey">排序约束</typeparam>
        /// <param name="where">过滤</param>
        /// <param name="orderby">排序</param>
        /// <returns>分页数据</returns>
        IQueryable<TEntity> Paging<TKey>(int pageSize, int pageIndex, out int totalCount,
            Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> orderby, bool isAsc);

        TEntity Add(TEntity model);

        TEntity Remove(TEntity model, bool isAddedEFContext);

        /// <summary>
        /// 负责自定义实体和指定了具体要更新的字段
        /// </summary>
        bool Update(TEntity model, string[] propertys);

        int SaveChanges();

    }
}
