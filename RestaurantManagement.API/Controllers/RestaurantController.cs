using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;
using RestaurantManagement.UTIL.Helper;
using RestaurantManagement.API.DTO;
using RestaurantManagement.DATA.Repository;
using RestaurantManagement.API.Mapper;
using RestaurantManagement.API.DTO.Restaurant;
using RestaurantManagement.API.DTO.Customer;

namespace RestaurantManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantManager _restaurantManager;
        public RestaurantController(RestaurantManager restaurantManager)
        {
            _restaurantManager = restaurantManager;
        }

        [HttpGet("Get/{restaurantId}/AvailableTables")]
        public async Task<ActionResult<List<Table>>> GetAvailableTables(string date, string hour, int restaurantId)
        {
            try
            {
                DateTime reservationDate = Parser.ParseDate(date);
                TimeSpan reservationHour = Parser.ParseTime(hour);

                // Retrieve the available tables based on the provided parameters
                List<Table> availableTables = await _restaurantManager.GetAvailableTablesAsync(reservationDate, reservationHour, restaurantId);

                if (availableTables.Count == 0)
                {
                    return NotFound(); // No available tables found
                }

                return Ok(availableTables);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/")]
        public async Task<ActionResult<List<Restaurant>>> GetRestaurants(int? postalCode, int? cuisineId)
        {
            List<RestaurantOutputDTO> restaurantOutputDTOs = new List<RestaurantOutputDTO>();
            try
            {
                List<Restaurant> restaurants = await _restaurantManager.GetRestaurantsAsync(postalCode, cuisineId);

                if (restaurants.Count == 0)
                {
                    return NotFound(); // No matching restaurants found
                }

                foreach (var restaurant in restaurants)
                {
                    RestaurantOutputDTO restaurantOutputDTO = RestaurantMapper.FromRestaurant(restaurant);
                    restaurantOutputDTOs.Add(restaurantOutputDTO);
                }

                return Ok(restaurantOutputDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{date}/Seats/{amountOfSeats}")]
        public async Task<ActionResult> GetRestaurants(string date, int amountOfSeats, int? postalCode, int? cuisineId)
        {
            List<RestaurantOutputDTO> restaurantOutputDTOs = new List<RestaurantOutputDTO>();
            try
            {
                DateTime paresedDate = Parser.ParseDate(date);

                List<Restaurant> restaurants = await _restaurantManager.GetRestaurantsAsync(paresedDate, amountOfSeats, postalCode, cuisineId);

                if (restaurants.Count == 0)
                {
                    return NotFound(); // No matching restaurants found
                }

                foreach (var restaurant in restaurants)
                {
                    RestaurantOutputDTO restaurantOutputDTO = RestaurantMapper.FromRestaurant(restaurant);
                    restaurantOutputDTOs.Add(restaurantOutputDTO);
                }

                return Ok(restaurantOutputDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Add")]
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
        [Route("Get/{restaurantId}")]
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
        [Route("Delete")]
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
    }
}
