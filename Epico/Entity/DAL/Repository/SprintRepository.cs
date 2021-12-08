using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Entity.DAL.Repository
{
    public class SprintRepository : BaseRepository<Sprint, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public SprintRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public new async Task<Sprint> GetById(int id)
        {
            return await _dbContext.Set<Sprint>()
                .Where(x => x.ID == id)
                .Include(x => x.Features)
                .FirstAsync();
        }

        public new async Task<List<Sprint>> GetByIds(List<int> ids)
        {
            return await _dbContext.Set<Sprint>()
                .Where(l => ids.Contains(l.ID))
                .Include(x => x.Features)
                .ToListAsync();
        }

        public new async Task<List<Sprint>> GetAll()
        {
            return await _dbContext.Set<Sprint>()
                .Include(x => x.Features)
                .ToListAsync();
        }

        public async Task<List<Sprint>> UpdateRange(List<Sprint> sprints)
        {
            foreach (var sprint in sprints)
            {
                _dbContext.Entry(sprint).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();
            return sprints;
        }
    }
}