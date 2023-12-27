using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Customer;
using RestaurantManagement.API.DTO.Location;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.TEST
{
    public class CustomerControllerTest
    {
        private readonly Mock<ICustomerRepository> customerMockRepo;
        private readonly Mock<IReservationRepository> reservationMockRepo;
        private readonly Mock<IRestaurantRepository> restaurantMockRepo;

        private readonly CustomerManager customerManager;
        private readonly ReservationManager reservationManager;
        private readonly RestaurantManager restaurantManager;

        private readonly CustomerController customerController;
    
        public CustomerControllerTest()
        {
            customerMockRepo = new Mock<ICustomerRepository>();
            reservationMockRepo = new Mock<IReservationRepository>();
            restaurantMockRepo = new Mock<IRestaurantRepository>();

            customerManager = new CustomerManager(customerMockRepo.Object);
            reservationManager = new ReservationManager(reservationMockRepo.Object, restaurantMockRepo.Object);
            restaurantManager = new RestaurantManager(restaurantMockRepo.Object);

            customerController = new CustomerController(customerManager , restaurantManager ,reservationManager);
        }

        #region Happy Paths

        [Fact]
        public async Task PostCustomer_ValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var customerInputDTO = new CustomerInputDTO
            {
                Name = "John", // Set the customer name based on your test case

                LocationInput = new LocationInputDTO
                {
                    PostalCode = 9940, // Set the postal code based on your test case
                    MunicipalityName = "Cityville", // Set the municipality name based on your test case
                    StreetName = "Main Street", // Set the street name based on your test case
                    HouseNumber = "123A" // Set the house number based on your test case
                },

                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "john.doe@example.com", // Set the email based on your test case
                    PhoneNumber = "0498531583" // Set the phone number based on your test case
                }
            };


            customerMockRepo.Setup(repo => repo.RegisterCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(1); // Assuming 1 is the customer number returned by the manager

            // Act
            var result = await customerController.PostCustomer(customerInputDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(CustomerController.GetCustomer), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["customerNumber"]);
        }

        [Fact]
        public async Task GetCustomer_ExistingCustomer_ReturnsOkResult()
        {
            // Arrange
            int customerNumber = 1;

            var existingCustomer = new Customer
            {

                Name = "John Doe",

                Location = new Location
                {
                    PostalCode = 1234,
                    MunicipalityName = "New York",
                    StreetName = "Broadway",
                    HouseNumber = "123A"
                },

                ContactInformation = new ContactInformation
                {
                    Email = "john.doe@example.com",
                    PhoneNumber = "123456789"
                }
            };

            customerMockRepo.Setup(repo => repo.GetCustomerAsync(customerNumber))
                .ReturnsAsync(existingCustomer);

            // Act
            var result = await customerController.GetCustomer(customerNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var customerOutputDTO = Assert.IsType<CustomerOutputDTO>(okResult.Value);
        }

        [Fact]
        public async Task DeleteCustomer_ExistingCustomer_ReturnsNoContent()
        {
            // Arrange
            int customerNumber = 1;
            customerMockRepo.Setup(repo => repo.IsValidCustomerAsync(customerNumber))
            .ReturnsAsync(true); // Set this to true if the customer is valid, false otherwise


            // Act
            var result = await customerController.DeleteCustomer(customerNumber);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutCustomer_ExistingCustomer_ReturnsOkResult()
        {
            // Arrange
            int customerNumber = 1;
            customerMockRepo.Setup(repo => repo.IsValidCustomerAsync(customerNumber))
            .ReturnsAsync(true);

            // Set properties for the update
            var customerInputDTO = new CustomerInputDTO
            {
                Name = "Updated Name", // Set the updated name
                LocationInput = new LocationInputDTO
                {
                    PostalCode = 56789, // Set the updated postal code
                    MunicipalityName = "Updated City",
                    StreetName = "Updated Street",
                    HouseNumber = "456B" // Set the updated house number
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "updated.email@example.com", // Set the updated email
                    PhoneNumber = "9876543210" // Set the updated phone number
                }
            };

            // Act
            var result = await customerController.PutCustomer(customerNumber, customerInputDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedCustomerDTO = Assert.IsType<CustomerOutputDTO>(okResult.Value);
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task PostCustomer_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var customerInputDTO = new CustomerInputDTO
            {
                Name = null, // Set an invalid value for the customer name
                LocationInput = new LocationInputDTO
                {
                    PostalCode = 0, // Set an invalid value for the postal code
                    MunicipalityName = "Cityville",
                    StreetName = "Main Street",
                    HouseNumber = "123A"
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "test@example.com", // Set an invalid value for the email address
                    PhoneNumber = "1234567890"
                }
            };

            customerMockRepo.Setup(repo => repo.RegisterCustomerAsync(It.IsAny<Customer>()))
                .ThrowsAsync(new Exception("error"));

            // Act
            var result = await customerController.PostCustomer(customerInputDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Name is empty", badRequestResult.Value);
        }

        [Fact]
        public async Task GetCustomer_NonExistingCustomer_ReturnsNotFound()
        {
            // Arrange
            int customerNumber = 1;
            customerMockRepo.Setup(repo => repo.GetCustomerAsync(customerNumber))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await customerController.GetCustomer(customerNumber);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_NonExistingCustomer_ReturnsNotFound()
        {
            // Arrange
            int customerNumber = 9000;
            customerMockRepo.Setup(repo => repo.IsValidCustomerAsync(customerNumber))
                .ReturnsAsync(false);

            // Act
            var result = await customerController.DeleteCustomer(customerNumber);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutCustomer_NonExistingCustomer_ReturnsNotFound()
        {
            // Arrange
            int customerNumber = 9000;
            customerMockRepo.Setup(repo => repo.IsValidCustomerAsync(customerNumber))
            .ReturnsAsync(false);

            // Set properties for the update (even though the customer doesn't exist)
            var customerInputDTO = new CustomerInputDTO
            {
                Name = "Updated Name", // Set a name for the non-existing customer
                LocationInput = new LocationInputDTO
                {
                    PostalCode = 56789, // Set a postal code for the non-existing customer
                    MunicipalityName = "Updated City",
                    StreetName = "Updated Street",
                    HouseNumber = "456B" // Set a house number for the non-existing customer
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "updated.email@example.com", // Set an email for the non-existing customer
                    PhoneNumber = "9876543210" // Set a phone number for the non-existing customer
                }
            };

            // Act
            var result = await customerController.PutCustomer(customerNumber, customerInputDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutCustomer_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            int customerNumber = 1;
            customerMockRepo.Setup(repo => repo.IsValidCustomerAsync(customerNumber))
                .ReturnsAsync(true); // Assume the customer is valid

            // Set properties for the update, but intentionally make them invalid
            var customerInputDTO = new CustomerInputDTO
            {
                Name = null, // Set an invalid name (null)
                LocationInput = new LocationInputDTO
                {
                    PostalCode = -1, // Set an invalid postal code (negative)
                    MunicipalityName = "Updated City",
                    StreetName = "Updated Street",
                    HouseNumber = "456B"
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "updated.email@example.com",
                    PhoneNumber = "invalidPhoneNumber" // Set an invalid phone number
                }
            };

            // Act
            var result = await customerController.PutCustomer(customerNumber, customerInputDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion
    }
}