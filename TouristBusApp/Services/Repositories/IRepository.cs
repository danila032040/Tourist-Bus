using System.Collections.Generic;
using TouristBusApp.Models;

namespace TouristBusApp.Services.Repositories
{
    /// <summary>
    ///     Класс репозитория, необходимый для CRUD(Create Read Update Delete) операций
    /// </summary>
    /// <typeparam name="T">Тип сущности, хранящийся в репозитории</typeparam>
    public interface IRepository<T> where T : IBaseEntity
    {
        /// <summary>
        ///     Создание сущности в репозитории
        /// </summary>
        /// <param name="entity">Создаваемая сущность, без заданного Id</param>
        public void Create(T entity);

        /// <summary>
        ///     Чтение всех сущностей из репозитория
        /// </summary>
        /// <returns>Сущности</returns>
        public IEnumerable<T> Read();

        /// <summary>
        ///     Обновление сущности из репозитория
        /// </summary>
        /// <param name="entity">Обновляемая сущность с Id сущности, которая уже существует в репозитории</param>
        public void Update(T entity);

        /// <summary>
        ///     Удаление сущности
        /// </summary>
        /// <param name="id">Id сущности, удаляемую из репозитория</param>
        public void Delete(int id);
    }
}