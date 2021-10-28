using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Entity.DAL.Repository
{
    public class TeamRepository : BaseRepository<Task, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public TeamRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}