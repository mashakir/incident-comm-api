using Incident.Comm.Integration.Data.Interfaces;
using Incident.Comm.Integration.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Incident.Comm.Integration.Data.Repositiories
{
    public abstract class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
    {
        protected readonly TContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(TContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual IQueryable<TEntity> GetById(Guid id)
        {
            return DbSet.Where(x => x.Id == id).AsNoTracking();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet.AsNoTracking();
        }

        public virtual void Update(TEntity entity)
        {
            entity.SetUpdated();
            DbSet.Update(entity);
        }

        public virtual void Remove(Guid id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.Where(filter)
                .AsNoTracking();
        }

        public IQueryable<TEntity> Get<TProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TProperty>> include)
        {
            return DbSet.Where(filter)
                .Include(include)
                .AsNoTracking();
        }

        public TEntity Get<TProperty>(Guid id, Expression<Func<TEntity, TProperty>> include)
        {
            return DbSet.Where(x => x.Id == id)
                .Include(include)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
        }
    }
}
