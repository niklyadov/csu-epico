using Epico.Entity;
using System.Threading.Tasks;

namespace Epico.Services
{
    public interface IFeatureService
    {
        public Task<Feature> AddFeature(string name, string description, string hypothesis, Team team, Metric metric);
    }
}
