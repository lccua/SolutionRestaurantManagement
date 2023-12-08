using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly CustomerManager _customerManager;
        private readonly RestaurantManager _restaurantManager;
        private readonly ReservationManager _reservationManager;
        public AdminController(CustomerManager customerManager, RestaurantManager restaurantManager, ReservationManager reservationManager)
        {
            _customerManager = customerManager;
            _restaurantManager = restaurantManager;
            _reservationManager = reservationManager;
        }

        [HttpPost]
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

        [HttpGet]
        [Route("{restaurantId}")]
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
        [Route("{restaurantId}")]
        public async Task<ActionResult> DeleteRestaurant(int restaurantId)
        {
            try
            {
                // Check if the customer exists
                var existingCustomer = await _restaurantManager.GetRestaurantAsync(restaurantId);
                if (existingCustomer == null)
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
        [Route("Restaurant")]
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
