using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Epico.Entity.DAL.Repository
{
    public class MetricRepository : BaseRepository<Metric, ApplicationContext>
    {
        private readonly ApplicationContext _dbContext;
        public MetricRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Metric> GetMetricById(int metricId)
        {
            return await _dbContext.Set<Metric>().Where(m => m.ID == metricId)
                .Include(x => x.Children)
                .FirstAsync();
        }
        
        public async Task<Metric> GetNsmMetric()
        {
            if (!await _dbContext.Set<Metric>().AnyAsync())
            {
                return null;
            }
        
            return await _dbContext.Set<Metric>()
                .Where(x => x.ParentMetricId != null || !x.ParentMetricId.HasValue )
                .Include(x => x.Children)
                    .ThenInclude(x=> x.Children)
                .FirstAsync();
        }
    }
}