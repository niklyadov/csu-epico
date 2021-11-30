using System.Collections.Generic;
using System.Threading.Tasks;
using Epico.Entity.DAL;
using Epico.Entity.DAL.Repository;

namespace Epico.Services
{
    public class BaseService<T, TContext> where TContext : ApplicationContext where T : class, IEntity
    {
        private readonly BaseRepository<T,TContext> _repository;
        protected BaseService(BaseRepository<T,TContext> repository)
        {
            _repository = repository;
        }
        
        public async Task<T> GetById(int id)
            => await _repository.GetById(id);

        public async Task<List<T>> GetByIds(List<int> ids)
        {
            if (ids == null) 
                return new List<T>();
            
            return await _repository.GetByIds(ids);
        }
        
        public async Task<List<T>> GetAll()
            => await _repository.GetAll();

        public async Task<int> Save(T entity)
            => await _repository.Save(entity);
        
        public async Task<T> Delete(int featureId)
            => await _repository.Delete(featureId);
        
        public async Task<T> Add(T entity)
            => await _repository.Add(entity);
        
        
        public async Task<T> Update(T entity)
            => await _repository.Update(entity);
    }
}