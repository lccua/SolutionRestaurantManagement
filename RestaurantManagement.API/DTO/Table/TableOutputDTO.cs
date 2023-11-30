using RestaurantManagement.API.DTO.Restaurant;

namespace RestaurantManagement.API.DTO.Table
{
    public class TableOutputDTO
    {
        public int TableNumber { get; set; }

        public int Capacity { get; set; }
        public RestaurantOutputDTO RestaurantOutput { get; set; }
    }
}
