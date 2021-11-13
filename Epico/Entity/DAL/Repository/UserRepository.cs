using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Epico.Entity.DAL.Repository
{
    public class UserRepository
    {
        private readonly ApplicationContext _dbContext;
        protected UserRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Set<User>().ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _dbContext.Set<User>().FindAsync(id);
        }

        public async Task<User> Add(User entity)
        {
            var entry = _dbContext.Set<User>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<User> Update(User entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Delete(int id)
        {
            var entity = await _dbContext.Set<User>().FindAsync(id);

            if (entity != null)
            {
                _dbContext.Set<User>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}
