using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Apps.IDAL
{
    public interface IBaseRepository<T>: IDisposable
    {
        bool Create(T model);
        bool Edit(T model);
        bool Delete(T model);
        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="keyValues"></param>
        int Delete(params object[] keyValues);
        T GetById(params object[] keyValues);
        /// <summary>
        /// 获得所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetList();
        /// <summary>
        /// 根据表达式获取数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetList<S>(int pageSize,int pageIndex,out int total
            ,Expression<Func<T,bool>> whereLambda,bool isAsc,Expression<Func<T,bool>> orderByLambda);

        bool IsExist(object id);
        int SaveChanges();
    }
}
