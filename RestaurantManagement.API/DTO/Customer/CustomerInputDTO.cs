using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.API.DTO.Location;

namespace RestaurantManagement.API.DTO.Customer
{
    public class CustomerInputDTO
    {

        public string Name { get; set; }
        public LocationInputDTO LocationInput { get; set; }
        public ContactInformationInputDTO ContactInformationInput { get; set; }
    }
}
