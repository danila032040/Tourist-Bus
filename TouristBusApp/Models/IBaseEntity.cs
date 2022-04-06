namespace TouristBusApp.Models
{
    public interface IBaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор сущности
        /// </summary>
        int Id { get; set; }
    }
}