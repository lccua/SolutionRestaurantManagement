using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.API.DTO;
using RestaurantManagement.API.Mapper;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {

        private readonly ReservationManager _reservationManager;
        public ReservationController(ReservationManager reservationManager)
        {
            _reservationManager = reservationManager;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> PostReservation(ReservationDTO reservationInputDTO)
        {
            try
            {
                // Map ReservationDTO to Reservation
                Reservation reservation = ReservationMapper.ToReservationDTO(reservationInputDTO);

                await _reservationManager.AddReservationAsync(reservation);

                ReservationDTO reservationOutputDTO = ReservationMapper.FromReservation(reservation);

                return CreatedAtAction(nameof(PostReservation), reservationOutputDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /* [HttpPut]
         [Route("Update")]
         public async Task<ActionResult> PutReservation(int customerId, int reservationNumber, DateOnly reservationDate, TimeOnly reservationHour, int amountOffSeats)
         {
             //give error if the amount offseats or reservation date or reservation hour doesnt work
         }

         [HttpDelete]
         [Route("Cancel")]
         public async Task<ActionResult> DeleteReservation(int customerId, int reservationNumber)
         {
             //reservation that has not passed yet
         }

         [HttpGet]
         [Route("Get")]
         public async Task<ActionResult> GetReservations(int CustomerId, DateOnly StartDate, DateOnly EndDate)
         {
             // Your code here
         }*/

    }
}
