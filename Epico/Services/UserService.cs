using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = Epico.Entity.Task;

namespace Epico.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        public UserService(UserRepository repository)
        {
            _repository = repository;
        }
        public async Task<User> AddUser(string firstName, string lastName)
        {
            return await _repository.Add(new User
            {
                UserName = string.Join(' ', firstName, lastName)
            });
        }

        public async Task<List<User>> GetUsersList()
        {
            // todo прикрутить пагинацию
            return await _repository.GetAll();
        }
    }
}
