using RestaurantManagement.API.DTO;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public static class LocationMapper
    {
        public static Location ToLocation(LocationDTO locationDTO)
        {
            try
            {
                return new Location
                {
                    Id = locationDTO.Id,
                    PostalCode = locationDTO.PostalCode,
                    MunicipalityName = locationDTO.MunicipalityName,
                    StreetName = locationDTO.StreetName,
                    HouseNumber = locationDTO.HouseNumber
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        public static LocationDTO FromLocation(Location location)
        {
            try
            {
                return new LocationDTO
                {
                    Id = location.Id,
                    PostalCode = location.PostalCode,
                    MunicipalityName = location.MunicipalityName,
                    StreetName = location.StreetName,
                    HouseNumber = location.HouseNumber
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
