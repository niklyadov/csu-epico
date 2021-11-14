using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        public TaskService(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<Entity.Task> AddTask(string name, string description, List<Feature> features, DateTime deadLine)
        {
            return await _taskRepository.Add(new Entity.Task
            {
                Name = name,
                Description = description,
                Features = features,
                DeadLine = deadLine,
                State = TaskState.NotStarted
            });
        }
    }
}
