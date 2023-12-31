﻿using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Location;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public static class LocationMapper
    {
        public static Location ToLocation(LocationInputDTO locationDTO)
        {
            try
            {
                return new Location
                {
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

        public static LocationOutputDTO FromLocation(Location location)
        {
            try
            {
                return new LocationOutputDTO
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
