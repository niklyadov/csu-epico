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
                //.Include(x => x.Metrics)
                .Include(x => x.Sprints)
                //.Include(x => x.Roadmaps)
                .SingleAsync();
        }

        public async Task<Project> AddMetricToProjectWithId(string ownerUserId, int projectId, Metric metric)
        {
            var project = await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId && p.ID == projectId)
                .SingleAsync();
            //project.Metrics ??= new List<Metric>();
            //project.Metrics.Add(metric);
            
            _dbContext.Entry(project).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();

            return project;
        }
        
        public async Task<Project> AddSprintToProjectWithId(string ownerUserId, int projectId, Sprint sprint)
        {
            var project = await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId && p.ID == projectId)
                .SingleAsync();
            project.Sprints ??= new List<Sprint>();
            project.Sprints.Add(sprint);
            
            _dbContext.Entry(project).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();

            return project;
        }
        
        public async Task<Project> AddRoadmapToProjectWithId(string ownerUserId, int projectId)
        {
            var project = await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId && p.ID == projectId)
                .SingleAsync();
            //project.Roadmaps ??= new List<Roadmap>();
            //project.Roadmaps.Add(roadmap);
            
            _dbContext.Entry(project).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();

            return project;
        }
    }
}