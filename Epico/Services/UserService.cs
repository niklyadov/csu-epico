using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class UserService : IDBservice<User>
    {
        private readonly UserRepository _repository;
        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<List<User>> GetByIds(List<int> ids)
        {
            return await _repository.GetByIds(ids);
        }

        public async Task<List<User>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<int> Save(User entity)
        {
            return await _repository.Save(entity);
        }

        public async Task<User> Delete(int entityId)
        {
            return await _repository.Delete(entityId);
        }

        public async Task<User> Add(User entity)
        {
            return await _repository.Add(entity);
        }

        public async Task<User> Update(User entity)
        {
            return await _repository.Update(entity);
        }
    }
}
