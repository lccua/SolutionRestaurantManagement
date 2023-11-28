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
                    Cuisine = CuisineMapper.ToCuisineDTO(restaurantDTO.CuisineDTO),
                    ContactInformation = ContactInformationMapper.ToContactInformation(restaurantDTO.ContactInformationDTO),
                    Location = LocationMapper.ToLocation(restaurantDTO.LocationDTO),
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
                    CuisineDTO = CuisineMapper.FromCuisine(restaurant.Cuisine),
                    ContactInformationDTO = ContactInformationMapper.FromContactInformation(restaurant.ContactInformation),
                    LocationDTO = LocationMapper.FromLocation(restaurant.Location),

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
