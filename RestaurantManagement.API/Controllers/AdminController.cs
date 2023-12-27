using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantManagement.API.DTO.Reservation;
using RestaurantManagement.API.DTO.Restaurant;
using RestaurantManagement.API.Mapper;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;
using RestaurantManagement.UTIL.Helper;

namespace RestaurantManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly RestaurantManager _restaurantManager;
        private readonly ReservationManager _reservationManager;

        public AdminController(RestaurantManager restaurantManager, ReservationManager reservationManager)
        {
            _restaurantManager = restaurantManager;
            _reservationManager = reservationManager;
        }

        #region Restaurant

        [HttpPost]
        [Route("Restaurant")]
        public async Task<ActionResult> PostRestaurant(RestaurantInputDTO restaurantInputDTO)
        {
            try
            {


                // Map CustomerDTO to Customer
                Restaurant restaurant = RestaurantMapper.ToRestaurantDTO(restaurantInputDTO);

                int restaurantId = await _restaurantManager.AddRestaurantAsync(restaurant);

                Restaurant restaurantOutputDTO = RestaurantMapper.ToRestaurantDTO(restaurantInputDTO);

                return CreatedAtAction(nameof(GetRestaurant), new { restaurantId }, restaurantOutputDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Restaurant/{restaurantId}")]
        public async Task<ActionResult> PutRestaurant(int restaurantId, RestaurantInputDTO restaurantInputDTO)
        {
            try
            {
                // Retrieve the customer based on customerNumber
                bool isValidRestaurant = await _restaurantManager.IsValidRestaurantAsync(restaurantId);

                if (isValidRestaurant == false)
                {
                    return NotFound(); // Customer not found
                }

                Restaurant restaurant = RestaurantMapper.ToRestaurantDTO(restaurantInputDTO);

                // Call the repository method to update the customer
                await _restaurantManager.UpdateRestaurantAsync(restaurantId, restaurant);

                RestaurantOutputDTO restaurantOutput = RestaurantMapper.FromRestaurant(restaurant);

                return Ok(restaurantOutput);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Restaurant/{restaurantId}")]
        public async Task<ActionResult> GetRestaurant(int restaurantId)
        {
            try
            {
                // Retrieve the customer based on customerNumber
                Restaurant restaurant = await _restaurantManager.GetRestaurantAsync(restaurantId);

                if (restaurant == null)
                {
                    return NotFound(); // Customer not found
                }

                // Map the customer to a DTO for the response
                RestaurantOutputDTO restaurantOutputDTO = RestaurantMapper.FromRestaurant(restaurant);

                return Ok(restaurantOutputDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Restaurant/{restaurantId}")]
        public async Task<ActionResult> DeleteRestaurant(int restaurantId)
        {
            try
            {
                // Check if the customer exists
                bool isValidRestaurant = await _restaurantManager.IsValidRestaurantAsync(restaurantId);
                if (isValidRestaurant == false)
                {
                    return NotFound();
                }

                await _restaurantManager.DeleteRestaurantAsync(restaurantId);

                return NoContent(); // Successful deletion, no content to return
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Restaurant/{restaurantId}/Reservations/Period")]
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
        [Route("Restaurant/{restaurantId}/Reservations/Day")]
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

        #endregion
    }
}
