using RestaurantManagement.API.DTO;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public class ReservationMapper
    {
        public static Reservation ToReservationDTO(ReservationDTO reservationDTO)
        {
            try
            {
                return new Reservation
                {
                    ReservationNumber = reservationDTO.ReservationNumber,
                    Date = reservationDTO.ReservationDate,
                    Hour = reservationDTO.ReservationHour,
                    AmountOfSeats = reservationDTO.AmountOffSeats,
                    CustomerNumber = reservationDTO.CustomerNumber,
                    RestaurantId = reservationDTO.RestaurantNumber,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static ReservationDTO FromReservation(Reservation reservation)
        {
            try
            {
                return new ReservationDTO
                {
                    ReservationNumber = reservation.ReservationNumber,
                    ReservationDate = reservation.Date,
                    ReservationHour = reservation.Hour,
                    AmountOffSeats = reservation.AmountOfSeats,
                    CustomerNumber = reservation.CustomerNumber,
                    RestaurantNumber = reservation.RestaurantId
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
