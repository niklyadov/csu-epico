using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class SprintService : IDBservice<Sprint>
    {
        private readonly SprintRepository _sprintRepository;
        public SprintService(SprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }
        
        public async Task<Sprint> Add(Sprint entity)
        {
            return await _sprintRepository.Add(entity);
        }

        public async Task<Sprint> Update(Sprint sprint)
        {
            return await _sprintRepository.Update(sprint);
        }

        public async Task<int> Save(Sprint entity)
        {
            return await _sprintRepository.Save(entity);
        }

        public async Task<Sprint> Delete(int id)
        {
            return await _sprintRepository.Delete(id);
        }

        public async Task<Sprint> GetById(int id)
        {
            return await _sprintRepository.GetById(id);
        }

        public async Task<List<Sprint>> GetByIds(List<int> ids)
        {
            if (ids == null)
                return new List<Sprint>();

            return await _sprintRepository.GetByIds(ids);
        }

        public async Task<List<Sprint>> GetAll()
        {
            return await _sprintRepository.GetAll();
        }
    }
}
