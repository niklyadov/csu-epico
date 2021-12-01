using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Epico.Entity.DAL;
using Task = Epico.Entity.Task;

namespace Epico.Services
{
    public class TaskService : BaseService<Task, ApplicationContext>
    {
        private readonly TaskRepository _taskRepository;
        public TaskService(TaskRepository taskRepository) : base(taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Task> AddTask(string name, string description, List<User> team, DateTime deadLine)
            => await _taskRepository.Add(new Task
            {
                Name = name,
                Description = description,
                DeadLine = deadLine,
                State = TaskState.NotStarted,
                Team = team
            });

        public new async Task<Task> GetById(int id)
        {
            return await _taskRepository.GetById(id);
        }

        public new async Task<List<Task>> GetByIds(List<int> ids)
        {
            if (ids == null)
                return new List<Task>();

            return await _taskRepository.GetByIds(ids);
        }

        public new async Task<List<Task>> GetAll()
        {
            return await _taskRepository.GetAll();
        }
    }
}
