using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Reservation;
using RestaurantManagement.DOMAIN.Model;
using RestaurantManagement.UTIL.Helper;

namespace RestaurantManagement.API.Mapper
{
    public class ReservationMapper
    {
        public static Reservation ToReservationDTO(ReservationInputDTO reservationDTO)
        {
            try
            {

                return new Reservation
                {
                    Date = Parser.ParseDate(reservationDTO.ReservationDate),
                    StartHour = Parser.ParseTime(reservationDTO.ReservationHour),
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

        public static Reservation ToReservationDTO(ReservationUpdateDTO reservationDTO)
        {
            try
            {

                return new Reservation
                {
                    Date = Parser.ParseDate(reservationDTO.ReservationDate),
                    StartHour = Parser.ParseTime(reservationDTO.ReservationHour),
                    AmountOfSeats = reservationDTO.AmountOffSeats,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static ReservationOutputDTO FromReservation(Reservation reservation)
        {
            try
            {
                return new ReservationOutputDTO
                {
                    ReservationDate = reservation.Date.ToString("yyyy-MM-dd"),
                    ReservationHour = reservation.StartHour.ToString(@"hh\:mm\"),
                    AmountOffSeats = reservation.AmountOfSeats,
                    CustomerNumber = reservation.CustomerNumber,
                    RestaurantNumber = reservation.RestaurantId,
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
