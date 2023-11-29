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

        Task<bool> IsValidReservationAsync(int reservationId);

        Task<Reservation> GetReservationAsync(int reservationNumber);


    }
}
