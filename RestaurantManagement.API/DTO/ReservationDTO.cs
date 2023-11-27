using System.Text.Json.Serialization;

namespace RestaurantManagement.API.DTO
{
    public class ReservationDTO
    {
        public ReservationDTO()
        {
            
        }

        [JsonIgnore]
        public int ReservationNumber { get; set; }

        public string ReservationDate { get; set; }
        public string ReservationHour { get; set; }

        public int AmountOffSeats { get; set; }

        public int CustomerNumber { get; set; }
        public int RestaurantNumber { get; set; }


    }
}
