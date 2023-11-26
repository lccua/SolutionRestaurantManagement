using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Exception.Model;


namespace RestaurantManagement.DOMAIN.Model
{
    public class Location
    {


        public Location()
        {

        }

        public Location(int id, int postalCode, string municipalityName, string streetName, string houseNumber)
        {
            _id = id;
            _postalCode = postalCode;
            _municipalityName = municipalityName;
            _streetName = streetName;
            _houseNumber = houseNumber;
        }

        private int _id;
        public int Id { get { return _id; } set { ValidateId(value); _id = value; } }

        private int _postalCode;
        public int PostalCode { get { return _postalCode; } set { ValidatePostalCode(value); _postalCode = value; } }

        private string _municipalityName;
        public string MunicipalityName { get { return _municipalityName; } set { ValidateMunicipalityName(value); _municipalityName = value; } }

        private string _streetName;
        public string StreetName { get { return _streetName; } set { ValidateStreetName(value); _streetName = value; } }

        private string _houseNumber;
        public string HouseNumber { get { return _houseNumber; } set { ValidateHouseNumber(value); _houseNumber = value; } }

        // Validation for the ID property
        private void ValidateId(int value)
        {
            if (value <= 0)
            {
                throw new LocationException("Invalid ID");
            }
        }

        // Validation for the PostalCode property
        private void ValidatePostalCode(int value)
        {
            // Add specific validation if needed
        }

        // Validation for the MunicipalityName property
        private void ValidateMunicipalityName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new LocationException("MunicipalityName is empty");
            }
        }

        // Validation for the HouseNumber property
        private void ValidateStreetName(string value)
        {
            // Add specific validation if needed
        }

        // Validation for the HouseNumber property
        private void ValidateHouseNumber(string value)
        {
            // Add specific validation if needed
        }
    }
}
