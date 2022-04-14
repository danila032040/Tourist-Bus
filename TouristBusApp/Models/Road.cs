using System;

namespace TouristBusApp.Models
{
    public class Road : IBaseEntity
    {
        /// <summary>
        ///     Отправная точка тура дороги
        /// </summary>
        public int DepartureTourPointId { get; set; }

        /// <summary>
        ///     Конечная точка тура дороги
        /// </summary>
        public int ArrivalTourPointId { get; set; }

        /// <summary>
        ///     Стоимость необходимая, чтобы проехать по этой дороги для тура
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        ///     Уникальный идентификатор дороги
        /// </summary>
        public int Id { get; set; }

        protected bool Equals(Road other)
        {
            return Id == other.Id &&
                   DepartureTourPointId == other.DepartureTourPointId &&
                   ArrivalTourPointId == other.ArrivalTourPointId &&
                   Price == other.Price;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Road) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, DepartureTourPointId, ArrivalTourPointId, Price);
        }
    }
}