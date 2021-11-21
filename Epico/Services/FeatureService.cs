using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class FeatureService
    {
        private readonly FeatureRepository _featureRepository;

        public FeatureService(FeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }
        
        public async Task<Feature> GetFeature(int featureId)
        {
            return await _featureRepository.GetById(featureId);
        }

        public async Task<Feature> UpdateFeature(Feature feature)
        {
            return await _featureRepository.Update(feature);
        }

        public async Task<List<Feature>> GetFeaturesList()
        {
            return await _featureRepository.GetAll();
        }
        
        public async Task<List<Feature>> GetFeaturesListByIds(List<int> ids)
        {
            return await _featureRepository.GetByIds(ids);
        }

        public async Task<Feature> DeleteFeature(int featureId)
        {
            return await _featureRepository.Delete(featureId);
        }
    }
}
