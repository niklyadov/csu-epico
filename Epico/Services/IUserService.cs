using Epico.Entity;
using System.Threading.Tasks;

namespace Epico.Services
{
    public interface IUserService
    {
        public Task<User> AddUser(string firstName, string lastName);
    }
}
