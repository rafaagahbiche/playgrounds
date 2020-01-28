using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaygroundsGallery.DataEF.Models;

namespace PlaygroundsGallery.DataEF.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly IGalleryContext MainDbContext;

		protected readonly DbSet<TEntity> EntityDbSet;
		public Repository(IGalleryContext dbContext)
		{
			MainDbContext = dbContext;
			EntityDbSet = dbContext.Set<TEntity>();
		}

		public async Task<bool> Add(TEntity entry)
		{
			entry.Created = DateTime.UtcNow;
			await EntityDbSet.AddAsync(entry);
			return await MainDbContext.SaveChangesAsync() > 0;
		}

		public async Task<bool> Update(TEntity entry)
		{
			entry.Updated = DateTime.UtcNow;
			EntityDbSet.Update(entry);
			return await MainDbContext.SaveChangesAsync() > 0;
		}

		public async Task<bool> Remove(TEntity entry) 
		{
			EntityDbSet.Remove(entry);
			return await MainDbContext.SaveChangesAsync() > 0;
		} 


        public async Task<IEnumerable<TEntity>> GetAll() => await EntityDbSet.ToListAsync();

        public async Task<TEntity> Get(int id) => await EntityDbSet.FindAsync(id);

        public virtual async Task<IEnumerable<TEntity>> Find(
			Expression<Func<TEntity, bool>> predicate = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			int take = -1,
			params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> query = EntityDbSet;
			
			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			var initQuery = query;
			foreach (var include in includeProperties)
			{
				initQuery = initQuery.Include(include);
			}

			var endQuery = initQuery ?? query;
			if (orderBy != null)
			{
				if (take > -1)
				{
					return await orderBy(endQuery).Take(take).ToListAsync();	
				}

				return await orderBy(endQuery).ToListAsync();
			}
			else
			{
				if (take > -1)
				{
					return await endQuery.Take(take).ToListAsync();	
				}

				return await endQuery.ToListAsync();
			}
		}

		public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate,
                                             params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> query = EntityDbSet;
			var initQuery = query;
			foreach (var include in includeProperties)
			{
				initQuery = initQuery.Include(include);
			}

			var endQuery = initQuery ?? query;
			return await endQuery.SingleOrDefaultAsync(predicate);
		}

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) => await EntityDbSet.AnyAsync(predicate);
    }
}