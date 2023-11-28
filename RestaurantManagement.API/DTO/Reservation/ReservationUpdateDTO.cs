namespace RestaurantManagement.API.DTO.Reservation
{
    public class ReservationUpdateDTO
    {
        public string ReservationDate { get; set; }
        public string ReservationHour { get; set; }
        public int AmountOffSeats { get; set; }
    }
}
