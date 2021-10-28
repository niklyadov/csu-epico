using System.Threading.Tasks;
using Epico.Entity;
using Microsoft.AspNetCore.Identity;
using Task = System.Threading.Tasks.Task;

namespace Epico.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Вход по логину паролю
        /// </summary>
        /// <param name="username">Логин пользователя </param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="persistent">Надо ли запоминать резулльтат входа</param>
        /// <returns>bool в зависимости от успеха</returns>
        public Task<bool> Login(string username, string password, bool persistent = true);

        /// <summary>
        /// Регистрация юзера
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Результат авторизации</returns>
        public Task<bool> Register(string username, string password);

        /// <summary>
        /// разлогинивание 🚬🗿
        /// </summary>
        public Task Logout();
    }
}