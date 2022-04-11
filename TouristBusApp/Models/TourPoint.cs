using System;

namespace TouristBusApp.Models
{
    public class TourPoint : IBaseEntity
    {
        /// <summary>
        ///     Название точки тура
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Уникальный идентификатор точки тура
        /// </summary>
        public int Id { get; set; }

        protected bool Equals(TourPoint other)
        {
            return Id == other.Id && Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TourPoint) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}