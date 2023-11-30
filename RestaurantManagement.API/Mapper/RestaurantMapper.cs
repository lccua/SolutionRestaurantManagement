using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Restaurant;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public class RestaurantMapper
    {
        public static Restaurant ToRestaurantDTO(RestaurantInputDTO restaurantDTO)
        {
            try
            {
                return new Restaurant
                {
                    Name = restaurantDTO.RestaurantName,
                    Cuisine = CuisineMapper.ToCuisineDTO(restaurantDTO.CuisineInput),
                    ContactInformation = ContactInformationMapper.ToContactInformation(restaurantDTO.ContactInformationInput),
                    Location = LocationMapper.ToLocation(restaurantDTO.LocationInput),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static RestaurantOutputDTO FromRestaurant(Restaurant restaurant)
        {
            try
            {
                return new RestaurantOutputDTO
                {
                    RestaurantId = restaurant.RestaurantId,
                    RestaurantName = restaurant.Name,
                    CuisineOutput = CuisineMapper.FromCuisine(restaurant.Cuisine),
                    ContactInformationOutput = ContactInformationMapper.FromContactInformation(restaurant.ContactInformation),
                    LocationOutput = LocationMapper.FromLocation(restaurant.Location),

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
