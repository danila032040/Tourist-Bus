using System;

namespace TouristBusApp.Models
{
    public class Road : IBaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор дороги
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Название дороги
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Отправная точка тура дороги
        /// </summary>
        public int DepartureTourPointId { get; set; }
        
        /// <summary>
        /// Конечная точка тура дороги
        /// </summary>
        public int ArrivalTourPointId { get; set; }
        
        /// <summary>
        /// Длинна дороги
        /// </summary>
        public int DistanceBetweenTourPoints { get; set; }

        protected bool Equals(Road other)
        {
            return Id == other.Id && 
                   Name == other.Name && 
                   DepartureTourPointId == other.DepartureTourPointId && 
                   ArrivalTourPointId == other.ArrivalTourPointId && 
                   DistanceBetweenTourPoints == other.DistanceBetweenTourPoints;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Road) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, DepartureTourPointId, ArrivalTourPointId, DistanceBetweenTourPoints);
        }
    }
}