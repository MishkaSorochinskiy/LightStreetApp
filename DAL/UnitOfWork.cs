using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork
    {
        public StreetManagementContext Context { get; }

        public UnitOfWork(StreetManagementContext dbContext)
        {
            Context = dbContext;
        }

        readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public GenericRepository<T> Repository<T>() where T : class, IEntity, new()
        {
            if (!_repositories.Keys.Contains(typeof(T)))
                _repositories.Add(typeof(T), new GenericRepository<T>(Context));
            return ((GenericRepository<T>)_repositories[typeof(T)]);
        }

        public int SaveChanges() => Context.SaveChanges();

        public Task<int> SaveChangesAsync() => Context.SaveChangesAsync();

        public IQueryable<TQuery> ExecuteQuery<TQuery>() where TQuery : class
        {
            return Context.Set<TQuery>().AsQueryable();
        }

        #region IDisposable
        bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
