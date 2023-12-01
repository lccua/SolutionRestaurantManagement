using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Customer;
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
                Reservation reservation = ReservationMapper.ToReservationDTO(reservationUpdateDTO);
                
                // Call the repository method to update the customer
                await _reservationManager.UpdateReservationAsync(reservationNumber,reservation);

                ReservationOutputDTO reservationOutput = ReservationMapper.FromReservation(reservation);

                return Ok(reservationOutput);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Cancel")]
        public async Task<ActionResult> DeleteReservation(int reservationNumber)
        {
            try
            {
                await _reservationManager.CancelReservationAsync(reservationNumber);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Customer/GetByPeriod")]
        public async Task<ActionResult> GetCustomerReservationsByPeriod(int customerNumber, string startDate, string endDate)
        {
            try
            {
                List<ReservationOutputDTO> reservationOutputDTOs = new List<ReservationOutputDTO>();
                DateTime parsedStartDate = Parser.ParseDate(startDate);
                DateTime parsedEndDate = Parser.ParseDate(endDate);

                List<Reservation> reservations = await _reservationManager.GetReservationsAsync(customerNumber, parsedStartDate, parsedEndDate);

                if (reservations.Count == 0)
                {
                    return NotFound(); // Customer not found
                }

                foreach (Reservation reservation in reservations)
                {
                    ReservationOutputDTO reservationOutputDTO = ReservationMapper.FromReservation(reservation);
                    reservationOutputDTOs.Add(reservationOutputDTO);
                }


                return Ok(reservationOutputDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Restaurant/GetByPeriod")]
        public async Task<ActionResult> GetRestaurantReservationsForPeriod(int restaurantId, string startDate, string endDate)
        {
            try
            {
                List<ReservationOutputDTO> reservationOutputDTOs = new List<ReservationOutputDTO>();
                DateTime parsedStartDate = Parser.ParseDate(startDate);
                DateTime parsedEndDate = Parser.ParseDate(endDate);

                List<Reservation> reservations = await _reservationManager.GetRestaurantReservationsForPeriodAsync(restaurantId, parsedStartDate, parsedEndDate);

                if (reservations.Count == 0)
                {
                    return NotFound(); // Customer not found
                }

                foreach (Reservation reservation in reservations)
                {
                    ReservationOutputDTO reservationOutputDTO = ReservationMapper.FromReservation(reservation);
                    reservationOutputDTOs.Add(reservationOutputDTO);
                }


                return Ok(reservationOutputDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Restaurant/GetByDay")]
        public async Task<ActionResult> GetRestaurantReservationsForDay(int restaurantId, string date)
        {
            try
            {
                List<ReservationOutputDTO> reservationOutputDTOs = new List<ReservationOutputDTO>();
                DateTime parsedDate = Parser.ParseDate(date);


                List<Reservation> reservations = await _reservationManager.GetRestaurantReservationsForDayAsync(restaurantId, parsedDate);

                if (reservations.Count == 0)
                {
                    return NotFound(); // Customer not found
                }

                foreach (Reservation reservation in reservations)
                {
                    ReservationOutputDTO reservationOutputDTO = ReservationMapper.FromReservation(reservation);
                    reservationOutputDTOs.Add(reservationOutputDTO);
                }


                return Ok(reservationOutputDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }
    }
}
