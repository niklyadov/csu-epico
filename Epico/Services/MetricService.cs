using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class MetricService
    {
        private MetricRepository _repository;
        public MetricService(MetricRepository repository)
        {
            _repository = repository;
        }

        public async Task<Metric> GetMetricById(int id)
        {
            return await _repository.GetById(id);
        }
    }
}
