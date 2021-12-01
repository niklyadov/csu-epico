using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class MetricService : IDBservice<Metric>
    {
        private readonly MetricRepository _metricRepository;
        public MetricService(MetricRepository repository) 
        {
            _metricRepository = repository;
        }
        
        public async Task<Metric> GetNsmMetric()
        {
            return await _metricRepository.GetNsmMetric();
        }

        public async Task<Metric> Update(Metric metric)
        {
            return await _metricRepository.Update(metric);
        }

        public async Task<int> Save(Metric entity)
        {
            return await _metricRepository.Save(entity);
        }

        public async Task<Metric> Delete(int id)
        {
            return await _metricRepository.Delete(id);
        }

        public async Task<Metric> Add(Metric metric)
        {
            return await _metricRepository.Add(metric);
        }

        public async Task<Metric> GetById(int id)
        {
            return await _metricRepository.GetById(id);
        }

        public async Task<List<Metric>> GetByIds(List<int> ids)
        {
            if (ids == null)
                return new List<Metric>();

            return await _metricRepository.GetByIds(ids);
        }

        public async Task<List<Metric>> GetAll()
        {
            return await _metricRepository.GetAll();
        }
    }
}
