using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Entity.DAL.Repository
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : ApplicationContext
    {
        private readonly TContext _dbContext;
        protected BaseRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetByIds(List<int> ids)
        {
            return await _dbContext.Set<TEntity>()
                .Where(l => ids.Contains(l.ID)).ToListAsync();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            var entry = _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> Save(TEntity entity)
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);

            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.Entry(entity).State = EntityState.Deleted;

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}