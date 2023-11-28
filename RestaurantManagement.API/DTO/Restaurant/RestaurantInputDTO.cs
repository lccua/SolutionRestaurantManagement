using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Cuisine;
using RestaurantManagement.API.DTO.Location;

namespace RestaurantManagement.API.DTO.Restaurant
{
    public class RestaurantInputDTO
    {

        public string RestaurantName { get; set; }
        public CuisineInputDTO CuisineDTO { get; set; }
        public ContactInformationInputDTO ContactInformationDTO { get; set; }
        public LocationInputDTO LocationDTO { get; set; }
    }
}
