using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
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

        public async Task<Feature> AddFeature(Feature feature)
        {
            try
            {
                return await _featureRepository.Add(feature);
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<Feature> GetFeatureById(int id)
        {
            return await _featureRepository.GetById(id);
        }

        public async Task<Feature> GetFeature(int featureId)
        {
            return await _featureRepository.GetById(featureId);
        }

        

        public async Task<List<Feature>> GetFeaturesList()
        {
            return await _featureRepository.GetAll();
        }
        
        public async Task<List<Feature>> GetFeaturesListByIds(List<int> ids)
        {
            if (ids == null) 
                return new List<Feature>();
            
            return await _featureRepository.GetByIds(ids);
        }

        public async Task<Feature> Delete(int id)
        {
            return await _featureRepository.Delete(id);
        }

        public async Task<Feature> Update(Feature feature)
        {
            return await _featureRepository.Update(feature);
        }

        public async Task<Feature> Add(Feature feature)
        {
            return await _featureRepository.Add(feature);
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
    }
}
