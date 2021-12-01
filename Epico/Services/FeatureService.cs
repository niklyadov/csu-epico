using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class FeatureService : IDBservice<Feature>
    {
        private readonly FeatureRepository _featureRepository;

        public FeatureService(FeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        public async Task<Feature> GetById(int id)
        {
            return await _featureRepository.GetById(id);
        }

        public async Task<List<Feature>> GetByIds(List<int> ids)
        {
            if (ids == null)
                return new List<Feature>();

            return await _featureRepository.GetByIds(ids);
        }

        public async Task<List<Feature>> GetAll()
        {
            return await _featureRepository.GetAll();
        }

        public async Task<int> Save(Feature entity)
        {
            return await _featureRepository.Save(entity);
        }

        public async Task<Feature> Delete(int featureId)
        {
            return await _featureRepository.Delete(featureId);
        }

        public async Task<Feature> Add(Feature entity)
        {
            return await _featureRepository.Add(entity);
        }

        public async Task<Feature> Update(Feature entity)
        {
            return await _featureRepository.Update(entity);
        }
    }
}
