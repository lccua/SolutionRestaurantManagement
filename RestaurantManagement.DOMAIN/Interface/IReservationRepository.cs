using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DOMAIN.Interface
{
    public interface IReservationRepository
    {
        Task AddReservationAsync(Reservation reservation);

        Task UpdateReservationAsync(int reservationNumber, Reservation reservation);

        Task<List<Reservation>> GetCustomerReservationsByPeriodAsync(int customerNumber, DateTime startDate, DateTime endDate);

        Task CancelReservationAsync(int reservationNumber);

        Task<List<Reservation>> GetRestaurantReservationsForPeriodAsync(int restaurantId, DateTime startDate, DateTime endDate);

        Task<List<Reservation>> GetRestaurantReservationsForDayAsync(int restaurantId, DateTime date);




    }
}
