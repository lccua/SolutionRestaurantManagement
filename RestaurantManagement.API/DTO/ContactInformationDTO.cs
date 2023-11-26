using System.Text.Json.Serialization;

namespace RestaurantManagement.API.DTO
{
    public class ContactInformationDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
