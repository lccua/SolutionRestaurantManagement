using System.Text.Json.Serialization;

namespace RestaurantManagement.API.DTO
{

    public class RestaurantDTO
    {
        [JsonIgnore]
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; }
        public CuisineDTO CuisineDTO { get; set; }
        public ContactInformationDTO ContactInformationDTO { get; set; }
    }
}
