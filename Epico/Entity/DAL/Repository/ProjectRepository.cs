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

        public new async Task<Project> GetById(int projectId)
        {
            return await _dbContext.Projects
                .Include(p => p.Sprints)
                    .ThenInclude(s => s.Features)
                .Include(p => p.Sprints)
                    .ThenInclude(s => s.Features)
                        .ThenInclude(f => f.Tasks)
                .Include(p => p.Sprints)
                    .ThenInclude(s => s.Features)
                        .ThenInclude(f => f.Metric)
                .SingleAsync(x => x.ID == projectId);
        }
        
        public async Task<List<Project>> GetUserProjects(string ownerUserId)
        {
            return await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId)
                .ToListAsync();
        }
        
        public async Task<int?> GetUserProjectId(string ownerUserId)
        {
            var project = await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId)
                .SingleAsync();

            if (project == null)
                return null;
            
            return project.ID;
        }

        public async Task<Project> AddMetricToProjectWithId(int projectId, Metric metric)
        {
            var project = await _dbContext.Projects
                .Where(p => p.ID == projectId)
                .SingleAsync();
            //project.Metrics ??= new List<Metric>();
            //project.Metrics.Add(metric);
            
            _dbContext.Entry(project).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();

            return project;
        }
        
        public async Task<Project> AddSprintToProjectWithId(int projectId, Sprint sprint)
        {
            var project = await _dbContext.Projects
                .Where(p => p.ID == projectId)
                .SingleAsync();
            project.Sprints ??= new List<Sprint>();
            project.Sprints.Add(sprint);
            
            _dbContext.Entry(project).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();

            return project;
        }
        
        public async Task<Project> AddRoadmapToProjectWithId(int projectId)
        {
            var project = await _dbContext.Projects
                .Where(p => p.ID == projectId)
                .SingleAsync();
            //project.Roadmaps ??= new List<Roadmap>();
            //project.Roadmaps.Add(roadmap);
            
            _dbContext.Entry(project).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();

            return project;
        }
    }
}