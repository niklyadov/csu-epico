using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public interface IDBservice<T>
    {
        public Task<T> GetById(int id);
        public Task<List<T>> GetByIds(List<int> ids);
        public Task<List<T>> GetAll();
        public Task<int> Save(T entity);
        public Task<T> Delete(int entityId);
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
    }
}