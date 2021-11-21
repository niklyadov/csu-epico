using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Epico.Entity.DAL.Repository
{
    public class SprintRepository : BaseRepository<Sprint, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public SprintRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sprint> AddFeature(int sprintId, Feature feature)
        {
            var sprint = await GetById(sprintId);

            if (sprint == null) return null;
            
            sprint.Features ??= new List<Feature>();
            sprint.Features.Add(feature);
                
            _dbContext.Entry(sprint).State = EntityState.Modified;
                
            await _dbContext.SaveChangesAsync();

            return sprint;
        }
    }
}