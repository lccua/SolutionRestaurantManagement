using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Cuisine;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public class CuisineMapper
    {
        public static Cuisine ToCuisineDTO(CuisineInputDTO cuisineDTO)
        {
            try
            {
                return new Cuisine
                {
                    CuisineType = cuisineDTO.CuisineType,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static CuisineOutputDTO FromCuisine(Cuisine cuisine)
        {
            try
            {
                return new CuisineOutputDTO
                {
                    CuisineId = cuisine.Id,
                    CuisineType = cuisine.CuisineType,


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
