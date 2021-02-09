using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DatabaseContext.Context
{
    public interface IUnitOfWork
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity Entity) where TEntity : class;

        int SaveChanges(Int64 MemberId);

        DatabaseContext GetInstance();
    }
}
