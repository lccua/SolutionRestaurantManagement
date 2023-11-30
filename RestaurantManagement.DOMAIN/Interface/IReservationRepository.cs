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

        Task<List<Reservation>> GetReservationsAsync(int customerNumber, DateTime startDate, DateTime endDate);

        Task CancelReservationAsync(int reservationNumber);


    }
}
