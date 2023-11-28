using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.ContactInformation;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public static class ContactInformationMapper
    {
        public static ContactInformation ToContactInformation(ContactInformationInputDTO contactInformationDTO)
        {
            try
            {
                return new ContactInformation
                {
                    Email = contactInformationDTO.Email,
                    PhoneNumber = contactInformationDTO.PhoneNumber
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        public static ContactInformationOutputDTO FromContactInformation(ContactInformation contactInformation)
        {
            try
            {
                return new ContactInformationOutputDTO
                {
                    Id = contactInformation.Id,
                    Email = contactInformation.Email,
                    PhoneNumber = contactInformation.PhoneNumber
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
