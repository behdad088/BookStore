using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using book.Infrastructure.Interfaces;
using book.Models;
using book.Repository;

namespace book.Infrastructure
{
    public class GenericRepository<TEntity>  : IGenericRepository<TEntity> where TEntity : class
    {
        internal RepositoryContext context;
        internal IDbSet<TEntity> dbSet;


        public GenericRepository(RepositoryContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null)
        {
            return await Get(filter, null, "");
        }
        public virtual async Task<IEnumerable<TEntity>> Get()
        {
            return await Get(null, null, "");
        }
        // Part 2
        public virtual async Task<IEnumerable<TEntity>> Get(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        
        public virtual async Task<TEntity> GetByID(int Id)
        {
            return dbSet.Find(Id);
        }

        public virtual async Task Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual async Task Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual ICollection<TEntity> GetAll()
        {
            return dbSet.AsEnumerable().ToList();
        }
        

        public virtual async Task Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual async Task Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
    }
}