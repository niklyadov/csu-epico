using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Epico.Entity;
using Epico.Entity.DAL.Repository;

namespace Epico.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly MetricRepository _metricRepository;
        public ProductService(ProductRepository productRepository, MetricRepository metricRepository)
        {
            _productRepository = productRepository;
            _metricRepository = metricRepository;
        }
        
        public async Task<Product> AddProduct(string name, string vision, string mission, string productFormula, string ownerUserId)
        {
            return await _productRepository.Add(new Product
            {
                Name = name,
                Vision = vision,
                Mission = mission,
                ProductFormula = productFormula,
                OwnerUserId = ownerUserId,
                Sprints = new List<Sprint>()
            }) ;
        }

        public async Task<Product> DeleteProduct(int projectId)
        {
            return await _productRepository.Delete(projectId);
        }

        public bool NotHasProduct()
        {
            return _productRepository.GetAll().Result?.Count == 0;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task<int?> UserProductId(string ownerUserId)
        {
            try
            {
                return await _productRepository.GetUserProductId(ownerUserId);
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }
        
        public async Task<Product> AddMetric(int projectId, Metric metric)
        {
            return await _productRepository.AddMetricToProductWithId(projectId , metric);
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