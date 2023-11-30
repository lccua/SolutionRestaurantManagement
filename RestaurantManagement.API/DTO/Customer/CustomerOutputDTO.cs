using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Location;

namespace RestaurantManagement.API.DTO.Customer
{
    public class CustomerOutputDTO
    {
        public int CustomerNumber { get; set; }

        public string Name { get; set; }
        public LocationOutputDTO LocationOutput { get; set; }
        public ContactInformationOutputDTO ContactInformationOutput { get; set; }
    }
}
