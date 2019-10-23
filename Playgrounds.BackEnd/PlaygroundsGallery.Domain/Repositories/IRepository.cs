using PlaygroundsGallery.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlaygroundsGallery.Domain.Repositories
{
	public interface IRepository<TEntity>
		where TEntity : Entity
	{
		Task<bool> Add(TEntity entry);

		Task<bool> Update(TEntity entity); 

		Task<IEnumerable<TEntity>> GetAll();

		Task<TEntity> Get(int id);

		void Remove(TEntity entry);

		Task<IEnumerable<TEntity>> Find(
			Expression<Func<TEntity, bool>> predicate = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			int take = -1,
			params Expression<Func<TEntity, object>>[] includeProperties);

		Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate,
		   params Expression<Func<TEntity, object>>[] includeProperties);

		Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate); 
    }
}
