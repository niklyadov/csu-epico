using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Entity.DAL.Repository
{
    public class FeatureRepository : BaseRepository<Feature, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public FeatureRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public new async Task<Feature> GetById(int id)
        {
            return await _dbContext.Set<Feature>()
                .Where(x => x.ID == id)
                .Include(x => x.Users)
                .Include(x => x.Tasks)
                .Include(x => x.Metric)
                .FirstAsync();
        }

        public new async Task<List<Feature>> GetByIds(List<int> ids)
        {
            return await _dbContext.Set<Feature>()
                .Where(l => ids.Contains(l.ID))
                .Include(x => x.Users)
                .Include(x => x.Tasks)
                .Include(x => x.Metric)
                .ToListAsync();
        }

        public new async Task<List<Feature>> GetAll()
        {
            return await _dbContext.Set<Feature>()
                .Include(x => x.Users)
                .Include(x => x.Tasks)
                .Include(x => x.Metric)
                .ToListAsync();
        }
    }
}