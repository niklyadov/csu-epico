using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Entity.DAL.Repository
{
    public class TeamRepository : BaseRepository<Team, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public TeamRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}