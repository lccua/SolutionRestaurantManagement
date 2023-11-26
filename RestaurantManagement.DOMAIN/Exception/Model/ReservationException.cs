using System.Runtime.Serialization;

namespace RestaurantManagement.DOMAIN.Exception.Model
{
    [Serializable]
    internal class ReservationException : System.Exception
    {
        public ReservationException()
        {
        }

        public ReservationException(string? message) : base(message)
        {
        }

        public ReservationException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected ReservationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}