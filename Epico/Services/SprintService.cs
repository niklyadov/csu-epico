using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class SprintService
    {
        private readonly SprintRepository _sprintRepository;
        public SprintService(SprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }
        public async Task<Sprint> AddSprint(string name, List<Entity.Task> tasks)
        {
            return await _sprintRepository.Add(new Sprint
            {
                Name = name
            });
        }
    }
}
