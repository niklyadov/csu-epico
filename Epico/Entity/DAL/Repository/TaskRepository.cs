using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Epico.Entity.DAL.Repository
{
    public class TaskRepository : BaseRepository<Task, ApplicationContext>
    {
        private ApplicationContext _dbContext;

        public TaskRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public new async Task<Task> GetById(int id)
        {
            return await _dbContext.Set<Task>()
                .Where(x => x.ID == id)
                .Include(x => x.ResponsibleUser)
                .FirstAsync();
        }

        public new async Task<List<Task>> GetByIds(List<int> ids)
        {
            return await _dbContext.Set<Task>()
                .Where(l => ids.Contains(l.ID))
                .Include(x => x.ResponsibleUser)
                .ToListAsync();
        }

        public new async Task<List<Task>> GetAll()
        {
            return await _dbContext.Set<Task>()
                .Include(x => x.ResponsibleUser)
                .ToListAsync();
        }

        public async Task<List<Task>> DeleteRange(List<Task> entites)
        {
            foreach (var item in entites)
            {
                var entity = await _dbContext.Set<Task>().FindAsync(item.ID);
                _dbContext.Set<Task>().Remove(entity);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            await _dbContext.SaveChangesAsync();

            return entites;
        }

        public async Task<List<Task>> UpdateRange(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                _dbContext.Entry(task).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();
            return tasks;
        }
    }
}