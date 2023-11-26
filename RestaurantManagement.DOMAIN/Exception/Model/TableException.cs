using System.Runtime.Serialization;

namespace RestaurantManagement.DOMAIN.Exception.Model
{
    [Serializable]
    internal class TableException : System.Exception
    {
        public TableException()
        {
        }

        public TableException(string? message) : base(message)
        {
        }

        public TableException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected TableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}