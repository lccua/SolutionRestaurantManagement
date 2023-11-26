using RestaurantManagement.API.DTO;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public static class CustomerMapper
    {
        public static Customer ToCustomerDTO(CustomerDTO customerDTO)
        {
            try
            {
                return new Customer
                {
                    CustomerNumber = customerDTO.CustomerNumber,
                    ContactInformation = ContactInformationMapper.ToContactInformation(customerDTO.ContactInformationDTO),
                    Location = LocationMapper.ToLocation(customerDTO.LocationDTO),
                    Name = customerDTO.Name,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static CustomerDTO FromCustomer(Customer customer)
        {
            try
            {
                return new CustomerDTO
                {
                    CustomerNumber = customer.CustomerNumber,
                    Name = customer.Name,
                    ContactInformationDTO = ContactInformationMapper.FromContactInformation(customer.ContactInformation),
                    LocationDTO = LocationMapper.FromLocation(customer.Location),
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
