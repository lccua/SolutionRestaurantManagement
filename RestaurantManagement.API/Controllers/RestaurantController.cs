using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;
using RestaurantManagement.UTIL.Helper;

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

        [HttpGet("Get/AvailableTables/Restaurant/{restaurantId}")]
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



        /*  [HttpGet]
          [Route("Get")]
          public async Task<ActionResult> GetRestaurant(int postalCode, int cuisineId)
          {

          }

          [HttpGet]
          [Route("GetFilteredRestaurants")]
          public async Task<ActionResult> GetRestaurants(DateOnly reservationDate, LocationDTO locationInputDTO, int cuisineId, int amountOfSeats)
          {

          }
  */

    }
}
