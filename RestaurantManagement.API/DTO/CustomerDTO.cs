using System.Text.Json.Serialization;

namespace RestaurantManagement.API.DTO
{
    public class CustomerDTO
    {
        [JsonIgnore]
        public int CustomerNumber { get; set; }

        public string Name { get; set; }
        public LocationDTO LocationDTO { get; set; }
        public ContactInformationDTO ContactInformationDTO { get; set; }


    }
}
