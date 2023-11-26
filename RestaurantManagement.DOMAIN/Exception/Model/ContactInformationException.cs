using System.Runtime.Serialization;

namespace RestaurantManagement.DOMAIN.Exception.Model
{
    [Serializable]
    internal class ContactInformationException : System.Exception
    {
        public ContactInformationException()
        {
        }

        public ContactInformationException(string? message) : base(message)
        {
        }

        public ContactInformationException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        protected ContactInformationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}