using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Customer;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public static class CustomerMapper
    {
        public static Customer ToCustomerDTO(CustomerInputDTO customerDTO)
        {
            try
            {
                return new Customer
                {
                    ContactInformation = ContactInformationMapper.ToContactInformation(customerDTO.ContactInformationInput),
                    Location = LocationMapper.ToLocation(customerDTO.LocationInput),
                    Name = customerDTO.Name,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static CustomerOutputDTO FromCustomer(Customer customer)
        {
            try
            {
                return new CustomerOutputDTO
                {
                    CustomerNumber = customer.CustomerNumber,
                    Name = customer.Name,
                    ContactInformationOutput = ContactInformationMapper.FromContactInformation(customer.ContactInformation),
                    LocationOutput = LocationMapper.FromLocation(customer.Location),
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
