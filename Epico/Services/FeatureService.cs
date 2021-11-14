using Epico.Entity;
using Epico.Entity.DAL.Repository;
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
        public async Task<Feature> AddFeature(string name, string description, string hypothesis, Team team, Metric metric)
        {
            return await _featureRepository.Add(new Feature
            {
                Name = name,
                Team = team,
                Metric = metric,
                Description = description,
                Hypothesis = hypothesis,
                State = FeatureState.NotStarted
            });
        }
    }
}
