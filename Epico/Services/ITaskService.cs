using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public interface ITaskService
    {
        public Task<Entity.Task> AddTask(string name, string description, List<Feature> features, DateTime deadLine);
    }
}
