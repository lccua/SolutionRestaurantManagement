using System.Text.Json.Serialization;

namespace RestaurantManagement.API.DTO
{
    public class ReservationDTO
    {
        [JsonIgnore]
        public int ReservationNumber { get; set; }

        public DateOnly ReservationDate { get; set; }
        public TimeOnly ReservationHour { get; set; }
        public int AmountOffSeats { get; set; }

        public int CustomerNumber { get; set; }
        public int RestaurantNumber { get; set; }
        public int TableNumber { get; set; }


    }
}
