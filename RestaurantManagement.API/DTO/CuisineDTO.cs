using System.Text.Json.Serialization;

namespace RestaurantManagement.API.DTO
{
    public class CuisineDTO
    {
        [JsonIgnore]
        public int CuisineId { get; set; }

        public string CuisineType { get; set; }
    }
}
