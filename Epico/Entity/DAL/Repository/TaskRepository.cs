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
                .Include(x => x.Team)
                .FirstAsync();
        }

        public new async Task<List<Task>> GetByIds(List<int> ids)
        {
            return await _dbContext.Set<Task>()
                .Where(l => ids.Contains(l.ID))
                .Include(x => x.Team)
                .ToListAsync();
        }

        public new async Task<List<Task>> GetAll()
        {
            return await _dbContext.Set<Task>()
                .Include(x => x.Team)
                .ToListAsync();
        }
    }
}