using System.Runtime.Serialization;

namespace RestaurantManagement.DOMAIN.Exception.Model
{
    [Serializable]
    internal class LocationException : System.Exception
    {
        public LocationException()
        {
        }

        public LocationException(string? message) : base(message)
        {
        }

        public LocationException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected LocationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}