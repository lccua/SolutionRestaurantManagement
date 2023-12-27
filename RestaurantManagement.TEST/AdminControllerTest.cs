using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.API.Controllers;
using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Cuisine;
using RestaurantManagement.API.DTO.Customer;
using RestaurantManagement.API.DTO.Location;
using RestaurantManagement.API.DTO.Reservation;
using RestaurantManagement.API.DTO.Restaurant;
using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.TEST
{
    public class AdminControllerTest
    {
        private readonly Mock<ICustomerRepository> customerMockRepo;
        private readonly Mock<IReservationRepository> reservationMockRepo;
        private readonly Mock<IRestaurantRepository> restaurantMockRepo;

        private readonly CustomerManager customerManager;
        private readonly ReservationManager reservationManager;
        private readonly RestaurantManager restaurantManager;

        private readonly AdminController adminController;

        public AdminControllerTest()
        {
            reservationMockRepo = new Mock<IReservationRepository>();
            restaurantMockRepo = new Mock<IRestaurantRepository>();

            reservationManager = new ReservationManager(reservationMockRepo.Object, restaurantMockRepo.Object);
            restaurantManager = new RestaurantManager(restaurantMockRepo.Object);

            adminController = new AdminController(restaurantManager, reservationManager);
        }

        #region Happy Paths

        [Fact]
        public async Task PostRestaurant_ValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var restaurantInputDTO = new RestaurantInputDTO
            {
                RestaurantName = "Restaurant Name",
                CuisineInput = new CuisineInputDTO
                {
                    CuisineType = "Italian" // Add the cuisine type as needed
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "restaurant@example.com", // Add the email address as needed
                    PhoneNumber = "1234567890" // Add the phone number as needed
                },
                LocationInput = new LocationInputDTO
                {
                    PostalCode = 12345, // Add the postal code as needed
                    MunicipalityName = "City", // Add the city name as needed
                    StreetName = "Street", // Add the street name as needed
                    HouseNumber = "123" // Add the house number as needed
                }
            };


            restaurantMockRepo.Setup(repo => repo.AddRestaurantAsync(It.IsAny<Restaurant>()))
                .ReturnsAsync(1); // Simulating a successful restaurant creation

            // Act
            var result = await adminController.PostRestaurant(restaurantInputDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(AdminController.GetRestaurant), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["restaurantId"]);
        }

        [Fact]
        public async Task PutRestaurant_ExistingRestaurant_ReturnsOkResult()
        {
            // Arrange
            int restaurantId = 1;
            restaurantMockRepo.Setup(repo => repo.IsValidRestaurantAsync(restaurantId))
            .ReturnsAsync(true);

            // Set properties for the update
            var restaurantInputDTO = new RestaurantInputDTO 
            {
                RestaurantName = "Updated Name", // Set the updated name

                CuisineInput = new CuisineInputDTO
                {
                    CuisineType = "Chinese" // Add the cuisine type as needed
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "updated.email@example.com", // Set the updated email
                    PhoneNumber = "9876543210" // Set the updated phone number
                },
                LocationInput = new LocationInputDTO
                {
                    PostalCode = 4564, // Set the updated postal code
                    MunicipalityName = "Updated City",
                    StreetName = "Updated Street",
                    HouseNumber = "456B" // Set the updated house number
                },
            };


            // Act
            var result = await adminController.PutRestaurant(restaurantId, restaurantInputDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedRestaurantDTO = Assert.IsType<RestaurantOutputDTO>(okResult.Value);
        }

        [Fact]
        public async Task GetRestaurant_ExistingRestaurant_ReturnsOkResult()
        {
            // Arrange
            int restaurantId = 1;
            var existingRestaurant = new Restaurant
            {
                Name = "La Trattoriat",

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
                },
                
                Cuisine = new Cuisine
                {
                    CuisineType = "Italian"
                }
            };

            restaurantMockRepo.Setup(repo => repo.GetRestaurantAsync(restaurantId))
                .ReturnsAsync(existingRestaurant);

            // Act
            var result = await adminController.GetRestaurant(restaurantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var restaurantOutputDTO = Assert.IsType<RestaurantOutputDTO>(okResult.Value);
        }

        [Fact]
        public async Task DeleteRestaurant_ExistingRestaurant_ReturnsNoContent()
        {
            // Arrange
            int restaurantId = 1;

            restaurantMockRepo.Setup(repo => repo.IsValidRestaurantAsync(restaurantId))
                .ReturnsAsync(true);
              

            // Act
            var result = await adminController.DeleteRestaurant(restaurantId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task PostRestaurant_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var restaurantInputDTO = new RestaurantInputDTO
            {
                RestaurantName = null, // Set an invalid value
                CuisineInput = new CuisineInputDTO
                {
                    CuisineType = "Italian" // Add the cuisine type as needed
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "restaurant@example.com", // Add the email address as needed
                    PhoneNumber = "1234567890" // Add the phone number as needed
                },
                LocationInput = new LocationInputDTO
                {
                    PostalCode = 12345, // Add the postal code as needed
                    MunicipalityName = "City", // Add the city name as needed
                    StreetName = "Street", // Add the street name as needed
                    HouseNumber = "123" // Add the house number as needed
                }
            };

            restaurantMockRepo.Setup(repo => repo.AddRestaurantAsync(It.IsAny<Restaurant>()))
              .ThrowsAsync(new Exception("error"));

            // Act
            var result = await adminController.PostRestaurant(restaurantInputDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Name is empty", badRequestResult.Value);
        }

        [Fact]
        public async Task GetRestaurant_NonExistingRestaurant_ReturnsNotFound()
        {
            // Arrange
            int restaurantId = 1;

            restaurantMockRepo.Setup(repo => repo.GetRestaurantAsync(restaurantId))
                .ReturnsAsync((Restaurant)null); // Simulating a non-existing restaurant

            // Act
            var result = await adminController.GetRestaurant(restaurantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteRestaurant_NonExistingRestaurant_ReturnsNotFound()
        {
            // Arrange
            int restaurantId = 9000;

            restaurantMockRepo.Setup(repo => repo.IsValidRestaurantAsync(restaurantId))
                .ReturnsAsync(false); // Simulating a non-existing restaurant

            // Act
            var result = await adminController.DeleteRestaurant(restaurantId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutRestaurant_NonExistingRestaurant_ReturnsNotFound()
        {
            // Arrange
            int restaurantId = 9000;
            restaurantMockRepo.Setup(repo => repo.IsValidRestaurantAsync(restaurantId))
            .ReturnsAsync(false);

            // Set properties for the update
            var restaurantInputDTO = new RestaurantInputDTO
            {
                RestaurantName = "Updated Name", // Set the updated name

                CuisineInput = new CuisineInputDTO
                {
                    CuisineType = "Chinese" // Add the cuisine type as needed
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "updated.email@example.com", // Set the updated email
                    PhoneNumber = "9876543210" // Set the updated phone number
                },
                LocationInput = new LocationInputDTO
                {
                    PostalCode = 4564, // Set the updated postal code
                    MunicipalityName = "Updated City",
                    StreetName = "Updated Street",
                    HouseNumber = "456B" // Set the updated house number
                },
            };

            // Act
            var result = await adminController.PutRestaurant(restaurantId, restaurantInputDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutRestaurant_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            int restaurantId = 1;
            restaurantMockRepo.Setup(repo => repo.IsValidRestaurantAsync(restaurantId))
            .ReturnsAsync(true);

            // Set properties for the update
            var restaurantInputDTO = new RestaurantInputDTO
            {
                RestaurantName = null, // Set the updated name

                CuisineInput = new CuisineInputDTO
                {
                    CuisineType = null, // Add the cuisine type as needed
                },
                ContactInformationInput = new ContactInformationInputDTO
                {
                    Email = "updated.email@example.com",
                    PhoneNumber = "invalidPhoneNumber" // Set an invalid phone number
                },

                LocationInput = new LocationInputDTO
                {
                    PostalCode = -1, // Set an invalid postal code (negative)
                    MunicipalityName = "Updated City",
                    StreetName = "Updated Street",
                    HouseNumber = "456B"
                },
            };

            // Act
            var result = await adminController.PutRestaurant(restaurantId, restaurantInputDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion
    }
}
