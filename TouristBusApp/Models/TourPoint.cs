namespace TouristBusApp.Models
{
    public class TourPoint
    {
        /// <summary>
        /// Название точки тура
        /// </summary>
        public string Name { get; set; }

        protected bool Equals(TourPoint other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TourPoint) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}