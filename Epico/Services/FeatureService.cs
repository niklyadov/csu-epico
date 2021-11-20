﻿using Epico.Entity;
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
        public async Task<Feature> AddFeature(string name, string description, string hypothesis, List<Entity.Task> tasks, List<Metric> metric)
        {
            return await _featureRepository.Add(new Feature
            {
                Name = name,
                Description = description,
                Hypothesis = hypothesis,
                Tasks = tasks,
                Metric = metric,
                State = FeatureState.NotStarted
            });
        }

        public async Task<List<Feature>> GetFeaturesList()
        {
            return await _featureRepository.GetAll();
        }
    }
}
