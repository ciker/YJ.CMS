using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace YJ.CMS.EFDAL
{
    public class BaseDAL<TEntity> : IDAL.IBaseDAL<TEntity> where TEntity : class
    {
        DbContext db = BaseDBContext.GetDBContext();

        DbSet<TEntity> _dbset;

        /// <summary>
        /// 实例化DbSet<TEntity>
        /// </summary>
        public BaseDAL()
        {
            _dbset = db.Set<TEntity>();
        }

        public DbSet<TEntity> DbSet
        {
            get
            {
                return _dbset;
            }
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="IncludeTables">要连表的数组集合</param>
        /// <returns></returns>
        public List<TEntity> WhereJoin(Expression<Func<TEntity, bool>> predicate, string[] IncludeTables)
        {
            DbQuery<TEntity> reslist = _dbset;  //DbSet的父类
            if (IncludeTables != null && IncludeTables.Length > 0)
            {
                foreach (var tbName in IncludeTables)
                {
                    reslist = reslist.Include(tbName);
                }
            }

            return reslist.Where(predicate).ToList();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            return _dbset.Where(where);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Paging<TKey>(int pageSize, int pageIndex, out int totalCount,
            Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> orderby, bool isAsc = true)
        {
            totalCount = _dbset.Where(where).Count();
            if (isAsc)
                return _dbset.Where(where).OrderBy(orderby).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            else
                return _dbset.Where(where).OrderByDescending(orderby).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public TEntity Add(TEntity model)
        {
            _dbset.Add(model);
            return model;
        }

        public TEntity Remove(TEntity model, bool isAddedEFContext)
        {
            //表示当前model未追加到EF上下文容器
            if (isAddedEFContext == false)
            {
                _dbset.Attach(model);
            }
            //将EF容器中当前实体对于的代理类状态修改成deleted
            _dbset.Remove(model);
            return model;
        }

        public bool Update(TEntity model, string[] propertys)
        {
            try
            {
                if (propertys == null || propertys.Length == 0)
                {
                    throw new Exception("当前更新的实体必须至少指定一个字段名称");
                }

                db.Configuration.ValidateOnSaveEnabled = false; //关闭EF的 实体验证检查
                DbEntityEntry entry = db.Entry(model);  //将实体附加到EF容器中
                //3.0 将EF容器中当前实体的代理类状态修改成Unchanged
                entry.State = System.Data.EntityState.Unchanged;
                //4.0 遍历用户传入的属性数值，将代理类中的属性对应的IsModified设置成true
                foreach (var item in propertys)
                {
                    entry.Property(item).IsModified = true;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }


    }
}