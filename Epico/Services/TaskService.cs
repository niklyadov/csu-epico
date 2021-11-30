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
    }
}
