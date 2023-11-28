namespace RestaurantManagement.API.DTO.Location
{
    public class LocationInputDTO
    {
        public int PostalCode { get; set; }
        public string MunicipalityName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
    }
}
