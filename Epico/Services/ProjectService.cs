using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Epico.Entity;
using Epico.Entity.DAL.Repository;

namespace Epico.Services
{
    public class ProjectService
    {
        private readonly ProjectRepository _projectRepository;
        private readonly MetricRepository _metricRepository;
        public ProjectService(ProjectRepository projectRepository, MetricRepository metricRepository)
        {
            _projectRepository = projectRepository;
            _metricRepository = metricRepository;
        }
        
        public async Task<Project> AddProject(string name, string vision, string mission, string productFormula, string ownerUserId)
        {
            return await _projectRepository.Add(new Project
            {
                Name = name,
                Vision = vision,
                Mission = mission,
                ProductFormula = productFormula,
                OwnerUserId = ownerUserId,
                Sprints = new List<Sprint>()
            }) ;
        }

        public async Task<Project> DeleteProject(int projectId)
        {
            return await _projectRepository.Delete(projectId);
        }

        public async Task<Project> GetProjectById(int id)
        {
            return await _projectRepository.GetById(id);
        }

        public async Task<int?> UserProjectId(string ownerUserId)
        {
            try
            {
                return await _projectRepository.GetUserProjectId(ownerUserId);
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }
        
        public async Task<Project> AddMetric(int projectId, Metric metric)
        {
            return await _projectRepository.AddMetricToProjectWithId(projectId , metric);
        }

        public async Task<Project> AddSprint(int projectId, Sprint sprint)
        {
            return await _projectRepository.AddSprintToProjectWithId(projectId, sprint);
        }
        
        public async Task<Project> AddRoadmap(int projectId)
        {
            return await _projectRepository.AddRoadmapToProjectWithId(projectId);
        }
    }
}