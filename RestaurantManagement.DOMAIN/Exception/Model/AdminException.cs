using System.Runtime.Serialization;

namespace RestaurantManagement.DOMAIN.Exception.Model
{
    [Serializable]
    internal class AdminException : System.Exception
    {
        public AdminException()
        {
        }

        public AdminException(string? message) : base(message)
        {
        }

        public AdminException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected AdminException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}