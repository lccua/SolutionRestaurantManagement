using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Reservation;
using RestaurantManagement.API.Mapper;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;
using RestaurantManagement.UTIL.Helper;

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
        public async Task<ActionResult> PostReservation(ReservationInputDTO reservationInputDTO)
        {
            try
            {


                // Map ReservationDTO to Reservation
                Reservation reservation = ReservationMapper.ToReservationDTO(reservationInputDTO);

                await _reservationManager.AddReservationAsync(reservation);

                ReservationOutputDTO reservationOutputDTO = ReservationMapper.FromReservation(reservation);

                return CreatedAtAction(nameof(PostReservation), reservationOutputDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> PutReservation(int reservationNumber,  ReservationUpdateDTO reservationUpdateDTO)
        {
            try
            {
                // Retrieve the existing customer
                bool isValid = await _reservationManager.IsValidReservationAsync(reservationNumber);

                if (!isValid)
                {
                    return NotFound(); // Customer not found
                }

                Reservation reservation = ReservationMapper.ToReservationDTO(reservationUpdateDTO);
                


                // Call the repository method to update the customer
                await _reservationManager.UpdateReservationAsync(reservationNumber,reservation);


                return Ok(reservationUpdateDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

     /*   [HttpDelete]
        [Route("Cancel")]
        public async Task<ActionResult> DeleteReservation(int customerId, int reservationNumber)
        {
            //reservation that has not passed yet
        }
*/


    }
}
