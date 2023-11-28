namespace RestaurantManagement.API.DTO.Reservation
{
    public class ReservationInputDTO
    {
        public string ReservationDate { get; set; }
        public string ReservationHour { get; set; }

        public int AmountOffSeats { get; set; }

        public int CustomerNumber { get; set; }
        public int RestaurantNumber { get; set; }

    }
}
