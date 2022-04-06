namespace TouristBusApp.Models
{
    public class User : IBaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }
        
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Роль пользователя
        /// </summary>
        /// <remarks>
        /// Необходимо для предоставление определенного
        /// функционала пользователю в зависимости от его роли
        /// </remarks>
        public UserRole Role { get; set; }
    }
}