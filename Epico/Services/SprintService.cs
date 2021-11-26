using System;
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
        
        public async Task<Sprint> GetSprintById(int id)
        {
            return await _sprintRepository.GetById(id);
        }
        
        public async Task<List<Sprint>> GetSprintList()
        {
            return await _sprintRepository.GetAll();
        }

        public async Task<Sprint> AddFeature(int sprintId, Feature feature)
        {
            try
            {
                return await _sprintRepository.AddFeature(sprintId, feature);
            }
            catch (Exception e)
            {
                
            }

            return null;
        }

        public async Task<Sprint> UpdateSprint(Sprint sprint)
        {
            return await _sprintRepository.Update(sprint);
        }

        public async Task<Sprint> DeleteSprint(int id)
        {
            return await _sprintRepository.Delete(id);
        }
    }
}
