using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Entity.DAL.Repository
{
    public class MetricRepository : BaseRepository<Metric, ApplicationContext>
    {
        private readonly ApplicationContext _dbContext;
        public MetricRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public new async Task<Metric> GetById(int id)
        {
            return await _dbContext.Set<Metric>()
                .Where(m => m.ID == id)
                .Include(x => x.Children)
                    .ThenInclude(x => x.Children)
                .FirstAsync();
        }

        public new async Task<List<Metric>> GetByIds(List<int> ids)
        {
            return await _dbContext.Set<Metric>()
                .Where(l => ids.Contains(l.ID))
                .Include(x => x.Children)
                    .ThenInclude(x => x.Children)
                .ToListAsync();
        }

        public new async Task<List<Metric>> GetAll()
        {
            return await _dbContext.Set<Metric>()
                .Include(x => x.Children)
                    .ThenInclude(x => x.Children)
                .ToListAsync();
        }

        public async Task<Metric> GetMetricTree()
        {
            if (!await _dbContext.Set<Metric>().AnyAsync())
            {
                return null;
            }
            var includeLevel = (await _dbContext.Set<Metric>()
                                .Where(x => x.ParentMetricId != null)
                                .ToListAsync())
                                    .Distinct()
                                    .Count();

            var metricTree = _dbContext.Set<Metric>()
                .Where(x => x.ParentMetricId != null || !x.ParentMetricId.HasValue)
                .Include(x => x.Children);

            for (int i = 0; i < includeLevel; i++)
                metricTree.ThenInclude(x => x.Children);

            return metricTree.First();
        }
    }
}