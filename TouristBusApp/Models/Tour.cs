using System;
using System.Collections.Generic;
using System.Linq;

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

        protected bool Equals(Tour other)
        {
            return Id == other.Id && 
                   Name == other.Name && 
                   Bus.Equals(other.Bus) && 
                   Departure.Equals(other.Departure) && 
                   Arrival.Equals(other.Arrival) && 
                   TourPoints.SequenceEqual(other.TourPoints);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Tour) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Bus, Departure, Arrival, TourPoints);
        }
    }
}