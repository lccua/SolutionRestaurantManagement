using System.Runtime.Serialization;

namespace RestaurantManagement.DOMAIN.Exception.Model
{
    [Serializable]
    internal class RestaurantException : System.Exception
    {
        public RestaurantException()
        {
        }

        public RestaurantException(string? message) : base(message)
        {
        }

        public RestaurantException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected RestaurantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}