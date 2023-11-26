using System.Text.Json.Serialization;

namespace RestaurantManagement.API.DTO
{
    public class TableDTO
    {
        [JsonIgnore]
        public int TableNumber { get; set; }

        public int Capacity { get; set; }
        public RestaurantDTO RestaurantDTO { get; set; }
    }
}
