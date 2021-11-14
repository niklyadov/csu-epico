using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Epico.Entity.DAL.Repository
{
    public class ProjectRepository : BaseRepository<Project, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public ProjectRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<List<Project>> GetUserProjects(string ownerUserId)
        {
            return await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId)
                .ToListAsync();
        }
        
        public async Task<Project> GetUserProjectWithId(string ownerUserId, int projectId)
        {
            return await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId && p.ID == projectId)
                .Include(x => x.Metrics)
                .Include(x => x.Sprints)
                .Include(x => x.Roadmaps)
                .SingleAsync();
        }

        public async Task<Project> AddMetricToProjectWithId(string ownerUserId, int projectId, Metric metric)
        {
            var project = await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId && p.ID == projectId)
                .SingleAsync();
            project.Metrics ??= new List<Metric>();
            project.Metrics.Add(metric);
            
            _dbContext.Entry(project).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();

            return project;
        }
    }
}