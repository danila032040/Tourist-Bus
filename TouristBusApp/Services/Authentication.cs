using System;
using System.Linq;
using TouristBusApp.Models;
using TouristBusApp.Services.Repositories;

namespace TouristBusApp.Services
{
    public class Authentication
    {
        private readonly IRepository<User> _rep;

        private User _signedInUser;

        public Authentication(IRepository<User> rep)
        {
            _rep = rep;
        }

        /// <summary>
        ///     Аутентификация пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <exception cref="Exception">Ошибка в случаи неверно логина или пароля</exception>
        public void SignIn(string login, string password)
        {
            _signedInUser = _rep.Read().FirstOrDefault(u => u.Login == login && u.Password == password);
            if (_signedInUser == null) throw new Exception("Неверный логин или пароль!!!");
        }

        /// <summary>
        ///     Выход
        /// </summary>
        public void SignOut()
        {
            _signedInUser = null;
        }

        /// <summary>
        ///     Получение аутентифицированного пользователя
        /// </summary>
        /// <returns>Аутентифицированный пользователь</returns>
        public User GetSignedInUser()
        {
            return _signedInUser;
        }
    }
}