using System;
using System.Collections.Generic;

namespace TouristBusApp.Models
{
    public class Tour : IBaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор тура
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Название тура
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Автобус, который будет ехать по туру
        /// </summary>
        public Bus Bus { get; set; }
        
        /// <summary>
        /// Дата отправления из стартовой точки
        /// </summary>
        public DateTime Departure { get; set; }
        
        /// <summary>
        /// Дата возвращения в стартовую точку тура
        /// </summary>
        public DateTime Arrival { get; set; }
        
        /// <summary>
        /// Маршрут тура, состоящий из нескольких точек тура
        /// </summary>
        public List<TourPoint> TourPoints { get; set; }

    }
}