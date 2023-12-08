using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Customer;
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
    public class CustomerController : ControllerBase
    {
        private readonly CustomerManager _customerManager;
        private readonly RestaurantManager _restaurantManager;
        private readonly ReservationManager _reservationManager;
        public CustomerController(CustomerManager customerManager, RestaurantManager restaurantManager, ReservationManager reservationManager)
        {
            _customerManager = customerManager;
            _restaurantManager = restaurantManager;
            _reservationManager = reservationManager;
        }

        [HttpPost]
        [Route("Customer")]
        public async Task<ActionResult> PostCustomer([FromBody] CustomerInputDTO customerInputDTO)
        {
            try
            {
                // Map CustomerDTO to Customer
                Customer customer = CustomerMapper.ToCustomerDTO(customerInputDTO);

                int customerNumber = await _customerManager.RegisterCustomerAsync(customer);

                CustomerOutputDTO customerOutputDTO = CustomerMapper.FromCustomer(customer);

                return CreatedAtAction(nameof(GetCustomer), new { customerNumber }, customerOutputDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{customerNumber}")]
        public async Task<IActionResult> DeleteCustomer(int customerNumber)
        {
            try
            {
                // Check if the customer exists
                bool customerIsValid = await _customerManager.IsValidCustomerAsync(customerNumber);
                if (customerIsValid == false)
                {
                    return NotFound();
                }

                await _customerManager.DeleteCustomerAsync(customerNumber);

                return NoContent(); // Successful deletion, no content to return
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{customerNumber}")]
        public async Task<ActionResult> PutCustomer(int customerNumber, [FromBody] CustomerInputDTO customerInputDTO)
        {
            try
            {
                // Retrieve the customer based on customerNumber
                bool isValidCustomer = await _customerManager.IsValidCustomerAsync(customerNumber);

                if (isValidCustomer == false)
                {
                    return NotFound(); // Customer not found
                }

                Customer customer = CustomerMapper.ToCustomerDTO(customerInputDTO);
              


                // Call the repository method to update the customer
                await _customerManager.UpdateCustomerAsync(customerNumber, customer);


                return Ok(customerInputDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{customerNumber}")]
        public async Task<IActionResult> GetCustomer(int customerNumber)
        {
            try
            {
                // Retrieve the customer based on customerNumber
                Customer customer = await _customerManager.GetCustomerAsync(customerNumber);

                if (customer == null)
                {
                    return NotFound(); // Customer not found
                }

                // Map the customer to a DTO for the response
                CustomerOutputDTO customerOutputDTO = CustomerMapper.FromCustomer(customer);

                return Ok(customerOutputDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("Seats")]
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

        [HttpGet]
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

        [HttpGet("{restaurantId}/AvailableTables")]
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


        [HttpPost]
        [Route("Reservation")]

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
        [Route("{reservationNumber}")]
        public async Task<ActionResult> PutReservation(int reservationNumber, ReservationUpdateDTO reservationUpdateDTO)
        {
            try
            {
                Reservation reservation = ReservationMapper.ToReservationDTO(reservationUpdateDTO);

                // Call the repository method to update the customer
                await _reservationManager.UpdateReservationAsync(reservationNumber, reservation);

                ReservationOutputDTO reservationOutput = ReservationMapper.FromReservation(reservation);

                return Ok(reservationOutput);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{reservationNumber}")]
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
        [Route("Reservations")]
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
    }
}
