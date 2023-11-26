using System.Text.Json.Serialization;

namespace RestaurantManagement.API.DTO
{
    public class LocationDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int PostalCode { get; set; }
        public string MunicipalityName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
    }
}
