using System.Collections.Generic;
using System.Threading.Tasks;
using Epico.Entity;
using Epico.Entity.DAL.Repository;

namespace Epico.Services
{
    public class ProjectService
    {
        private readonly ProjectRepository _repository;
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
                OwnerUserId = ownerUserId,
                Sprints = new List<Sprint>()
            }) ;
        }

        public async Task<Project> GetProjectById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<List<Project>> UserProjects(string ownerUserId)
        {
            return await _repository.GetUserProjects(ownerUserId);
        }

        public async Task<Project> UserProject(string ownerUserId, int id)
        {
            return await _repository.GetUserProjectWithId(ownerUserId, id);
        }
        
        public async Task<Project> AddMetric(string ownerUserId, int projectId, Metric metric)
        {
            return await _repository.AddMetricToProjectWithId(ownerUserId, projectId , metric);
        }
        
        public async Task<Project> AddSprint(string ownerUserId, int projectId, Sprint sprint)
        {
            return await _repository.AddSprintToProjectWithId(ownerUserId, projectId, sprint);
        }
        
        public async Task<Project> AddRoadmap(string ownerUserId, int projectId)
        {
            return await _repository.AddRoadmapToProjectWithId(ownerUserId, projectId);
        }
    }
}