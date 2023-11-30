using RestaurantManagement.API.DTO.Customer;
using RestaurantManagement.API.DTO.Restaurant;

namespace RestaurantManagement.API.DTO.Reservation
{
    public class ReservationOutputDTO
    {
        public int ReservationNumber { get; set; }

        public string ReservationDate { get; set; }
        public string ReservationHour { get; set; }

        public int AmountOffSeats { get; set; }

        public CustomerOutputDTO CustomerOutput { get; set; }
        public RestaurantOutputDTO RestaurantOutput { get; set; }
    }
}
