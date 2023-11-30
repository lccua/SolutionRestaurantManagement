using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Cuisine;
using RestaurantManagement.API.DTO.Location;

namespace RestaurantManagement.API.DTO.Restaurant
{
    public class RestaurantInputDTO
    {

        public string RestaurantName { get; set; }
        public CuisineInputDTO CuisineInput { get; set; }
        public ContactInformationInputDTO ContactInformationInput { get; set; }
        public LocationInputDTO LocationInput { get; set; }
    }
}
