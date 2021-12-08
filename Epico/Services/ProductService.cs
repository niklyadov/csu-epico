using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class ProductService : IDBservice<Product>
    {
        private readonly ProductRepository _productRepository;
        public ProductService(ProductRepository productRepository, MetricRepository metricRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProduct()
        {
            var list = await _productRepository.GetAll();

            return !list.Any() ? null : list.First();
        }

        public async Task<Product> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task<List<Product>> GetByIds(List<int> ids)
        {
            return await _productRepository.GetByIds(ids);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<int> Save(Product entity)
        {
            return await _productRepository.Save(entity);
        }

        public async Task<Product> Delete(int entityId)
        {
            return await _productRepository.Delete(entityId);
        }

        public async Task<Product> Add(Product entity)
        {
            return await _productRepository.Add(entity);
        }

        public async Task<Product> Update(Product entity)
        {
            return await _productRepository.Update(entity);
        }

        public async Task<Product> AddMetric(int projectId, Metric metric)
        {
            return await _productRepository.AddMetricToProductWithId(projectId, metric);
        }

        public async Task<Product> AddSprint(int projectId, Sprint sprint)
        {
            return await _productRepository.AddSprintToProductWithId(projectId, sprint);
        }

        public async Task<Product> AddRoadmap(int projectId)
        {
            return await _productRepository.AddRoadmapToProductWithId(projectId);
        }

    }
}