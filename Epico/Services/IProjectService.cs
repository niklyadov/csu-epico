using System.Collections.Generic;
using System.Threading.Tasks;
using Epico.Entity;

namespace Epico.Services
{
    public interface IProjectService
    {
        public Task<Project> AddProject(string name, string vision, string mission, string productFormula, string ownerUserId);
        public Task<Project> GetProjectById(int id);
        Task<List<Project> > UserProjects(string ownerUserId);
        Task<Project> UserProject(string ownerUserId, int id);
        void AddMetric(string ownerUserId, int id, Metric metric);
    }
}