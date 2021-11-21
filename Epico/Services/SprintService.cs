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

        public async Task<Sprint> UpdateSprint(int id, string name, List<Feature> features)
        {
            // todo прикрутить изменение спринта в базе
            return null;
        }

        public async Task<Sprint> DeleteSprint(int id)
        {
            // todo прикрутить удаление спринта из базы
            return null;
        }
    }
}
