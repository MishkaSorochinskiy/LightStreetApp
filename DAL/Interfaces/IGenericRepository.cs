using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity, new()
    {
        TEntity GetById(object id);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool asNoTracking = true);

        TEntity Insert(TEntity obj);

        void Update(TEntity obj);

        void Delete(TEntity id);
    }
}
