namespace TouristBusApp.Models
{
    public class Bus : IBaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор автобуса
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Номер автобуса
        /// </summary>
        public string Number { get; set; }
        
        /// <summary>
        /// Вместительность автобуса
        /// </summary>
        public int Capacity { get; set; }

    }
}