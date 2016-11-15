using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YJ.CMS.Model;

namespace YJ.CMS.BLL
{
    public abstract class BaseService<T> where T : class
    {
        ////简单工厂
        //protected IDAL.IUserInfoDAL dal = Factory.SimpleFac.GetUserInfoInstance();
        ////抽象工厂
        //protected IDAL.IUserInfoDAL dal = Factory.AbstractFac.GetUserInfoInstance();

        /*
         * 获取BaseDAL 让子类来赋值
         * 
         */
        protected IDAL.IBaseDAL<T> CurrentDal;

        #region 子类重写方法实现
        //public BaseService()
        //{
        //    SetCurrentDAL();
        //}

        ///// <summary>
        ///// 让子类重写
        ///// </summary>
        //public abstract void SetCurrentDAL();
        #endregion      

        public DbSet<T> DbSet
        {
            get
            {
                return CurrentDal.DbSet;
            }
        }

        public List<T> WhereJoin(Expression<Func<T, bool>> predicate, string[] IncludeTables)
        {
            return CurrentDal.WhereJoin(predicate, IncludeTables);            
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return CurrentDal.Query(where);
        }

        public IQueryable<T> Paging<TKey>(int pageSize, int pageIndex, out int totalCount, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderby, bool isAsc)
        {
            return CurrentDal.Paging(pageSize, pageIndex, out totalCount, where, orderby, isAsc);
        }

        public T Add(T model)
        {
            return CurrentDal.Add(model);
        }

        public T Remove(T model, bool isAddedEFContext)
        {
            return CurrentDal.Remove(model, isAddedEFContext);
        }

        public bool Update(T model, string[] propertys)
        {
            return CurrentDal.Update(model, propertys);
        }

        public int SaveChanges()
        {
            return CurrentDal.SaveChanges();
        }
    }
}
