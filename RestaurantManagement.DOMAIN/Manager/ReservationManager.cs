using RestaurantManagement.DOMAIN.Interface;
using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DOMAIN.Manager
{
    public class ReservationManager
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationManager(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task AddReservationAsync(Reservation reservation)
        {

        }
    }
}
