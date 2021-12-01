using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = Epico.Entity.Task;

namespace Epico.Services
{
    public class TaskService : IDBservice<Task>
    {
        private readonly TaskRepository _taskRepository;
        public TaskService(TaskRepository taskRepository)
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


        public async Task<Task> GetById(int id)
        {
            return await _taskRepository.GetById(id);
        }

        public async Task<List<Task>> GetByIds(List<int> ids)
        {
            return await _taskRepository.GetByIds(ids);
        }

        public async Task<List<Task>> GetAll()
        {
            return await _taskRepository.GetAll();
        }

        public async Task<int> Save(Task entity)
        {
            return await _taskRepository.Save(entity);
        }

        public async Task<Task> Delete(int taskId)
        {
            return await _taskRepository.Delete(taskId);
        }

        public async Task<Task> Add(Task entity)
        {
            return await _taskRepository.Add(entity);
        }

        public async Task<Task> Update(Task entity)
        {
            return await _taskRepository.Update(entity);
        }
    }
}
