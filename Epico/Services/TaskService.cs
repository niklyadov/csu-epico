﻿using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = Epico.Entity.Task;

namespace Epico.Services
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        public TaskService(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<Entity.Task> AddTask(string name, string description, List<User> team, DateTime deadLine)
        {
            try
            {
                return await _taskRepository.Add(new Entity.Task
                {
                    Name = name,
                    Description = description,
                    DeadLine = deadLine,
                    State = TaskState.NotStarted,
                    Team = team
                });
            } catch (Exception exception)
            {
                // логирование :D
            }

            return null;
        }
        
        public async Task<Task> GetTaskById(int id)
        {
            return await _taskRepository.GetById(id);
        }

        public async Task<Entity.Task> UpdateTask(Task task)
        {
            return await _taskRepository.Update(task);
        }

        public async Task<Entity.Task> DeleteTask(int id)
        {
            return await _taskRepository.Delete(id);
        }

        public async Task<List<Task>> GetTaskList()
        {
            return await _taskRepository.GetAll();
        }
        
        public async Task<List<Task>> GetTaskListByIds(List<int> ids)
        {
            if (ids == null) 
                return new List<Task>();
            
            return await _taskRepository.GetByIds(ids);
        }
    }
}
