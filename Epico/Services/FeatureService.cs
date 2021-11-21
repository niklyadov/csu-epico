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

        public async Task<Feature> UpdateFeature(string name, string description, string hypothesis, List<Entity.Task> tasks, List<Metric> metric)
        {
            // todo прикрутить обновление в базе
            return null;
        }

        public async Task<List<Feature>> GetFeaturesList()
        {
            return await _featureRepository.GetAll();
        }

        public async Task<Feature> DeleteFeature(int featureId)
        {
            // todo прикрутить удаление фичи из базы
            return null;
        }
    }
}
