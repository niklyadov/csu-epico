using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class UserService : IUserService
    {
        private UserRepository _repository;
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
    }
}
