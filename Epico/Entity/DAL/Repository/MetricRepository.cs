using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Entity.DAL.Repository
{
    public class MetricRepository : BaseRepository<Metric, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public MetricRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}