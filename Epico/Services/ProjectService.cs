using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epico.Entity;
using Epico.Entity.DAL;
using Epico.Entity.DAL.Repository;
using Task = Epico.Entity.Task;

namespace Epico.Services
{
    public class ProjectService : IProjectService
    {
        private ProjectRepository _repository;
        public ProjectService(ProjectRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Project> AddProject(string name, string vision, string mission, string productFormula, string ownerUserId)
        {
            return await _repository.Add(new Project
            {
                Name = name,
                Vision = vision,
                Mission = mission,
                ProductFormula = productFormula,
                OwnerUserID = ownerUserId
            });
        }

        public async Task<Project> GetProjectById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<List<Project>> UserProjects(string ownerUserId)
        {
            return await _repository.GetUserProjects(ownerUserId);
        }
    }
}