using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Cuisine;
using RestaurantManagement.API.DTO.Location;

namespace RestaurantManagement.API.DTO.Restaurant
{
    public class RestaurantOutputDTO
    {
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; }
        public CuisineOutputDTO CuisineOutput { get; set; }
        public ContactInformationOutputDTO ContactInformationOutput { get; set; }
        public LocationOutputDTO LocationOutput { get; set; }
    }
}
