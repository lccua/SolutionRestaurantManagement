using RestaurantManagement.API.DTO.Restaurant;

namespace RestaurantManagement.API.DTO.Table
{
    public class TableInputDTO
    {

        public int Capacity { get; set; }
        public RestaurantInputDTO RestaurantInput { get; set; }

    }
}
