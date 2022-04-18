using System;

namespace TouristBusApp.Models
{
    public class TourRequest : IBaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор заявки на тур
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Пользователь подавший заявку
        /// </summary>
        public int UserId { get; set; }
    
        /// <summary>
        /// Тур на который была подана заявка
        /// </summary>
        public int TourId { get; set; }

        protected bool Equals(TourRequest other)
        {
            return Id == other.Id && UserId == other.UserId && TourId == other.TourId;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TourRequest) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, UserId, TourId);
        }
    }
}