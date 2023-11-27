using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;
using RestaurantManagement.UTIL.Helper;
using RestaurantManagement.API.DTO;
using RestaurantManagement.DATA.Repository;
using RestaurantManagement.API.Mapper;

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
        public async Task<ActionResult<List<Restaurant>>> GetRestaurant(int? postalCode, int? cuisineId)
        {
            List<RestaurantDTO> restaurantDTOs = new List<RestaurantDTO>();
            try
            {
                List<Restaurant> restaurants = await _restaurantManager.GetRestaurantsAsync(postalCode, cuisineId);

                if (restaurants.Count == 0)
                {
                    return NotFound(); // No matching restaurants found
                }

                foreach (var restaurant in restaurants)
                {
                    RestaurantDTO restaurantDTO = RestaurantMapper.FromRestaurant(restaurant);
                    restaurantDTOs.Add(restaurantDTO);
                }

                return Ok(restaurantDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }

        /*[HttpGet]
        [Route("GetFilteredRestaurants")]
        public async Task<ActionResult> GetRestaurants(DateOnly reservationDate, LocationDTO locationInputDTO, int cuisineId, int amountOfSeats)
        {

        }*/


    }
}
