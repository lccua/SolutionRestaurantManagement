using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Customer;
using RestaurantManagement.API.Mapper;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerManager _customerManager;
        public CustomerController(CustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        [HttpPost]
        [Route("Add")]
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

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCustomer(int customerNumber)
        {
            try
            {
                // Check if the customer exists
                var existingCustomer = await _customerManager.GetCustomerAsync(customerNumber);
                if (existingCustomer == null)
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

        [HttpPut("Update")]
        public async Task<ActionResult> PutCustomer(int customerNumber, [FromBody] CustomerInputDTO customerInputDTO)
        {
            try
            {
                // Retrieve the customer based on customerNumber
                Customer existingCustomer = await _customerManager.GetCustomerAsync(customerNumber);

                if (existingCustomer == null)
                {
                    return NotFound(); // Customer not found
                }

                Customer customer = CustomerMapper.ToCustomerDTO(customerInputDTO);
                customer.ContactInformation.Id = existingCustomer.ContactInformation.Id;
                customer.CustomerNumber = customerNumber;
                customer.Location.Id = existingCustomer.Location.Id;



                // Call the repository method to update the customer
                await _customerManager.UpdateCustomerAsync(customer);


                return Ok(customerInputDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get/{customerNumber}")]
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
    }
}
