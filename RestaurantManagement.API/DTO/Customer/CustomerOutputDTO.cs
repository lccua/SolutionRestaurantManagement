using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Location;

namespace RestaurantManagement.API.DTO.Customer
{
    public class CustomerOutputDTO
    {
        public int CustomerNumber { get; set; }

        public string Name { get; set; }
        public LocationOutputDTO LocationDTO { get; set; }
        public ContactInformationOutputDTO ContactInformationDTO { get; set; }
    }
}
