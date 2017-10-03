using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace book.Infrastructure.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null);
        Task<IEnumerable<TEntity>> Get();
        Task<IEnumerable<TEntity>> Get(
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                string includeProperties = "");

        Task<TEntity> GetByID(int Id);
        Task Insert(TEntity entity);
        Task Update(TEntity entityToUpdate);
        ICollection<TEntity> GetAll();
        Task Delete(object id);
        Task Delete(TEntity entityToDelete);
    }
}
