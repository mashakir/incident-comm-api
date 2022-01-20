using System;
using System.Linq;
using System.Linq.Expressions;

namespace Incident.Comm.Integration.Data.Interfaces
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        void Add(TEntity entity);

        IQueryable<TEntity> GetById(Guid id);

        IQueryable<TEntity> GetAll();

        void Update(TEntity entity);

        void Remove(Guid id);

        int SaveChanges();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        TEntity Get<TProperty>(Guid id, Expression<Func<TEntity, TProperty>> include);

        IQueryable<TEntity> Get<TProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProperty>> include);
    }
}
