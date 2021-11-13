using Epico.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public interface ISprintService
    {
        public Task<Sprint> AddSprint(string name, List<Entity.Task> tasks);
    }
}
