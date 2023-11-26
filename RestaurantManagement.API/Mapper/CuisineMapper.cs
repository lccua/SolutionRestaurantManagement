using RestaurantManagement.API.DTO;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public class CuisineMapper
    {
        public static Cuisine ToCuisineDTO(CuisineDTO cuisineDTO)
        {
            try
            {
                return new Cuisine
                {
                    Id = cuisineDTO.CuisineId,
                    CuisineType = cuisineDTO.CuisineType,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static CuisineDTO FromCuisine(Cuisine cuisine)
        {
            try
            {
                return new CuisineDTO
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
