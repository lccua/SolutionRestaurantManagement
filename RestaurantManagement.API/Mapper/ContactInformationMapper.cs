using RestaurantManagement.API.DTO;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public static class ContactInformationMapper
    {
        public static ContactInformation ToContactInformation(ContactInformationDTO contactInformationDTO)
        {
            try
            {
                return new ContactInformation
                {
                    Id = contactInformationDTO.Id,
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

        public static ContactInformationDTO FromContactInformation(ContactInformation contactInformation)
        {
            try
            {
                return new ContactInformationDTO
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
