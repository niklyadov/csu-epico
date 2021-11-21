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
        private readonly MetricRepository _metricRepository;
        public MetricService(MetricRepository repository)
        {
            _metricRepository = repository;
        }

        public async Task<Metric> DeleteMetric(int id)
        {
            // todo прикрутить удаление метрики из базы
            return null;
        }

        public async Task<Metric> GetMetricById(int id)
        {
            return await _metricRepository.GetById(id);
        }
        
        public async Task<List<Metric>> GetMetricList()
        {
            return await _metricRepository.GetAll();
        }
        
        public async Task<List<Metric>> GetMetricListByIds(List<int> ids)
        {
            return await _metricRepository.GetByIds(ids);
        }
    }
}
