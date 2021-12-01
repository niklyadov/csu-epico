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

        public async Task<Metric> AddMetric(Metric metric)
        {
            try
            {
                return await _metricRepository.Add(metric);
            }
            catch (Exception exception)
            {
                // логирование :D
            }

            return metric;
        }

       

        public async Task<Metric> GetMetricById(int id)
        {
            return await _metricRepository.GetById(id);
        }
        
        public async Task<Metric> GetNsmMetric()
        {
            return await _metricRepository.GetNsmMetric();
        }
        
        public async Task<List<Metric>> GetMetricListByIds(List<int> ids)
        {
            if (ids == null) 
                return new List<Metric>();

            return await _metricRepository.GetByIds(ids);
        }
        
        public async Task<List<Metric>> GetMetricList()
        {
            return await _metricRepository.GetAll();
        }




        public async Task<Metric> Update(Metric metric)
        {
            return await _metricRepository.Update(metric);
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
