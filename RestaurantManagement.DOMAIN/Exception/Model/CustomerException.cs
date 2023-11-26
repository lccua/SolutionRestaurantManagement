using System.Runtime.Serialization;

namespace RestaurantManagement.DOMAIN.Exception.Model
{
    [Serializable]
    internal class CustomerException : System.Exception
    {
        public CustomerException()
        {
        }

        public CustomerException(string? message) : base(message)
        {
        }

        public CustomerException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected CustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}