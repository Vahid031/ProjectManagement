using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Services.Interfaces;
using DatabaseContext.Context;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastracture.AppCode;

namespace Services.EfServices
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        public readonly IUnitOfWork _uow;
        public readonly IDbSet<T> _dbSet;

        public GenericService(IUnitOfWork uow)
        {
            _uow = uow;
            _dbSet = _uow.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query) as IQueryable<T>;
            }
            else
            {
                return query as IQueryable<T>;
            }
        }

        public IQueryable<T> Get(Paging pg, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (pg._filter != "")
            {
                query = query.Where(pg._filter, pg._values.ToArray());
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (pg._orderColumn != null)
            {
                return query.OrderBy(pg._orderColumn);
            }
            else
            {
                return query as IQueryable<T>;
            }
        }

        public T Find(object Id)
        {
            return _dbSet.Find(Id);
        }


        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            if (_uow.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _uow.Entry(entity).State = EntityState.Modified;

            //foreach (var p in typeof(T).GetProperties())
            //{
            //    var Attributes = p.GetCustomAttributes(false);
            //    bool hasScaffold = false;

            //    foreach (var attribute in Attributes)
            //    {
            //        if (attribute.GetType() == typeof(NotMappedAttribute))
            //        {
            //            hasScaffold = true;
            //            break;
            //        }
            //    }
            //    if (hasScaffold)
            //        continue;

            //    var v = p.GetValue(entity, null);

            //    if (v == null)
            //        continue;

            //      if (p.Name != "Id")
            //        _uow.Entry(entity).Property(p.Name).IsModified = true;
            //}
        }

        public void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);

            Delete(entityToDelete);
        }

        public void Delete(T entity)
        {
            if (_uow.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public int GetCount(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
  
        }
    }
}