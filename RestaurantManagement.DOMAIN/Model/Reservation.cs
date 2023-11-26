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

        private DateOnly _date;
        public DateOnly Date { get { return _date; } set { ValidateDate(value); _date = value; } }

        private TimeOnly _hour;
        public TimeOnly Hour { get { return _hour; } set { ValidateHour(value); _hour = value; } }

        private int _tableNumber;
        public int TableNumber { get { return _tableNumber; } set { ValidateTableNumber(value); _tableNumber = value; } }

        private int _customerNumber;
        public int CustomerNumber { get { return _customerNumber; } set { ValidateCustomerNumber(value); _customerNumber = value; } }

        private int _restaurantId;
        public int RestaurantId { get { return _restaurantId; } set { ValidateRestaurantId(value); _restaurantId = value; } }

        public Customer Customer { get; set; }

        public Restaurant Restaurant { get; set; }

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

        private void ValidateDate(DateOnly value)
        {
            // Add specific validation if needed
        }

        private void ValidateHour(TimeOnly value)
        {
            // Add specific validation if needed
        }

        private void ValidateTableNumber(int value)
        {
            // Add specific validation if needed
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
    }
}
