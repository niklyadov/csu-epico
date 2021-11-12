using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class MetricService : IMetricService
    {
        private MetricRepository _repository;
        public MetricService(MetricRepository repository)
        {
            _repository = repository;
        }
        public async Task<Metric> AddMetric(string name, string description, int? parentMetricId)
        {
            Metric metric;
            if (parentMetricId.HasValue)
            {
                metric = new Metric
                {
                    Name = name,
                    Description = description,
                    ParentMetric = await GetMetricById(parentMetricId.Value)
                };
            }
            else
            {
                metric = new Metric
                {
                    Name = name,
                    Description = description
                };
            }
            return await _repository.Add(metric);
        }

        public async Task<Metric> GetMetricById(int id)
        {
            return await _repository.GetById(id);
        }
    }
}
