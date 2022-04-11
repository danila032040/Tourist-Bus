using System;

namespace TouristBusApp.Models
{
    public class User : IBaseEntity
    {
        /// <summary>
        ///     Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///     Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Роль пользователя
        /// </summary>
        /// <remarks>
        ///     Необходимо для предоставление определенного
        ///     функционала пользователю в зависимости от его роли
        /// </remarks>
        public UserRole Role { get; set; }

        /// <summary>
        ///     Уникальный идентификатор пользователя
        /// </summary>
        public int Id { get; set; }

        protected bool Equals(User other)
        {
            return Id == other.Id &&
                   Login == other.Login &&
                   Password == other.Password &&
                   Role == other.Role;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Login, Password, (int) Role);
        }
    }
}