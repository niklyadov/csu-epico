using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Epico.Entity.DAL.Repository
{
    public class ProductRepository : BaseRepository<Product, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public ProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public new async Task<List<Product>> GetAll()
        {
            return await _dbContext.Projects.ToListAsync();
        }

        public new async Task<Product> GetById(int projectId)
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
        
        public async Task<List<Product>> GetUserProjects(string ownerUserId)
        {
            return await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId)
                .ToListAsync();
        }
        
        public async Task<int?> GetUserProductId(string ownerUserId)
        {
            var project = await _dbContext.Projects
                .Where(p => p.OwnerUserId == ownerUserId)
                .SingleAsync();

            if (project == null)
                return null;
            
            return project.ID;
        }

        public async Task<Product> AddMetricToProductWithId(int projectId, Metric metric)
        {
            var project = await _dbContext.Projects
                .Where(p => p.ID == projectId)
                .SingleAsync();
            
            _dbContext.Entry(project).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync();

            return project;
        }
        
        public async Task<Product> AddSprintToProductWithId(int projectId, Sprint sprint)
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
        
        public async Task<Product> AddRoadmapToProductWithId(int projectId)
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