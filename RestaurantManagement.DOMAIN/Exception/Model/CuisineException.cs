using System.Runtime.Serialization;

namespace RestaurantManagement.DOMAIN.Exception.Model
{
    [Serializable]
    internal class CuisineException : System.Exception
    {
        public CuisineException()
        {
        }

        public CuisineException(string? message) : base(message)
        {
        }

        public CuisineException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected CuisineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}