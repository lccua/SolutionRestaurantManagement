using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Output
{
    public class ReservationOutputUI
    {
        public int reservationNumber { get; set; }
        public string reservationDate { get; set; }
        public string reservationHour { get; set; }
        public int amountOffSeats { get; set; }
        public CustomerOutputUI customerOutput { get; set; }
        public RestaurantOutputUI restaurantOutput { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Reservation Number: {reservationNumber}");
            builder.AppendLine($"Date: {reservationDate}");
            builder.AppendLine($"Hour: {reservationHour}");
            builder.AppendLine($"Seats: {amountOffSeats}");
            builder.AppendLine($"Customer: {customerOutput}");
            builder.AppendLine($"Restaurant: {restaurantOutput}");
            return builder.ToString();
        }
    }
}
