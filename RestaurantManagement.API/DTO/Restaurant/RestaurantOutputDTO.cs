using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Cuisine;
using RestaurantManagement.API.DTO.Location;

namespace RestaurantManagement.API.DTO.Restaurant
{
    public class RestaurantOutputDTO
    {
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; }
        public CuisineOutputDTO CuisineDTO { get; set; }
        public ContactInformationOutputDTO ContactInformationDTO { get; set; }
        public LocationOutputDTO LocationDTO { get; set; }
    }
}
