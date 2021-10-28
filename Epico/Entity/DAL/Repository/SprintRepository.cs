using System.Collections.Generic;
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
    }
}