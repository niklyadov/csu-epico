using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Entity.DAL.Repository
{
    public class RoadmapRepository : BaseRepository<Roadmap, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public RoadmapRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}