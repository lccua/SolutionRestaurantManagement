using RestaurantManagement.API.DTO;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public class RestaurantMapper
    {
        public static Restaurant ToRestaurantDTO(RestaurantDTO restaurantDTO)
        {
            try
            {
                return new Restaurant
                {
                    RestaurantId = restaurantDTO.RestaurantId,
                    Name = restaurantDTO.RestaurantName,
                    Cuisine = CuisineMapper.ToCuisineDTO(restaurantDTO.CuisineDTO),
                    ContactInformation = ContactInformationMapper.ToContactInformation(restaurantDTO.ContactInformationDTO),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static RestaurantDTO FromRestaurant(Restaurant restaurant)
        {
            try
            {
                return new RestaurantDTO
                {
                    RestaurantId = restaurant.RestaurantId,
                    RestaurantName = restaurant.Name,
                    CuisineDTO = CuisineMapper.FromCuisine(restaurant.Cuisine),
                    ContactInformationDTO = ContactInformationMapper.FromContactInformation(restaurant.ContactInformation),

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
