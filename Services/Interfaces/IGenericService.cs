using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        
        IQueryable<T> Get(Paging pg, string includeProperties = null);

        T Find(object Id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(object id);

        void Delete(T entity);

        int GetCount(Expression<Func<T, bool>> filter = null);
    }
}
