using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Exception.Model;

namespace RestaurantManagement.DOMAIN.Model
{
    public class Reservation
    {
        private int _reservationNumber;
        public int ReservationNumber { get { return _reservationNumber; } set { ValidateReservationNumber(value); _reservationNumber = value; } }

        private int _amountOfSeats;
        public int AmountOfSeats { get { return _amountOfSeats; } set { ValidateAmountOfSeats(value); _amountOfSeats = value; } }

        private DateTime _date;
        public DateTime Date { get { return _date; } set { ValidateDate(value); _date = value; } }

        private TimeSpan _startHour;
        public TimeSpan StartHour { get { return _startHour; } set { ValidateHour(value); _startHour = value; CalculateEndHour(); } }

        private TimeSpan _endHour;
        public TimeSpan EndHour { get { return _endHour; } set { ValidateHour(value); _endHour = value;}}

        private int _tableNumber;
        public int TableNumber { get { return _tableNumber; } set { ValidateTableNumber(value); _tableNumber = value; } }

        private int _customerNumber;
        public int CustomerNumber { get { return _customerNumber; } set { ValidateCustomerNumber(value); _customerNumber = value; } }

        private int _restaurantId;
        public int RestaurantId { get { return _restaurantId; } set { ValidateRestaurantId(value); _restaurantId = value; } }




        public Customer Customer { get; set; }

        public Restaurant Restaurant { get; set; }

        public Table Table { get; set; }

        private void ValidateReservationNumber(int value)
        {
            if (value <= 0)
            {
                throw new ReservationException("Invalid ReservationNumber");
            }
        }

        private void ValidateAmountOfSeats(int value)
        {
            if (value <= 0)
            {
                throw new ReservationException("Invalid AmountOfSeats");
            }
        }

       

        private void ValidateDate(DateTime value)
        {
            // Example validation: Date should be in the future
            if (value.Date < DateTime.Now.Date)
            {
                throw new ReservationException("Invalid reservation date. Please choose a date in the future.");
            }
        }

        private void ValidateHour(TimeSpan value)
        {
           
            // Allow only exact hours or half-hours
            int minutes = value.Minutes;
            if (minutes != 0 && minutes != 30)
            {
                throw new ReservationException("Invalid reservation hour. Only exact hours or half-hours are allowed.");
            }
        }


        private void ValidateTableNumber(int value)
        {
            if (value <= 0)
            {
                throw new ReservationException("Invalid ReservationNumber");
            }
        }

        private void ValidateCustomerNumber(int value)
        {
            if (value <= 0)
            {
                throw new ReservationException("Invalid CustomerNumber");
            }
        }

        private void ValidateRestaurantId(int value)
        {

            if (value <= 0)
            {
                throw new ReservationException("Invalid RestaurantId");
            }
        }

        private void CalculateEndHour()
        {
            _endHour = StartHour.Add(TimeSpan.FromHours(1.5));
        }
    }
}
