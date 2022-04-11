using System;

namespace TouristBusApp.Models
{
    public class Bus : IBaseEntity
    {
        /// <summary>
        ///     Номер автобуса
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        ///     Вместительность автобуса
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        ///     Уникальный идентификатор автобуса
        /// </summary>
        public int Id { get; set; }

        protected bool Equals(Bus other)
        {
            return Id == other.Id && Number == other.Number && Capacity == other.Capacity;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Bus) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Number, Capacity);
        }
    }
}