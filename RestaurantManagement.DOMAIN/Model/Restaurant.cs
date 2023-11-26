using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Exception.Model;

namespace RestaurantManagement.DOMAIN.Model
{
    public class Restaurant
    {
        public Restaurant()
        {

        }

        private int _restaurantId;
        public int RestaurantId { get { return _restaurantId; } set { ValidateRestaurantId(value); _restaurantId = value; } }

        private string _name;
        public string Name { get { return _name; } set { ValidateName(value); _name = value; } }

        private int _cuisineId;
        public int CuisineId { get { return _cuisineId; } set { ValidateCuisineId(value); _cuisineId = value; } }

        private int _locationId;
        public int LocationId { get { return _locationId; } set { ValidateLocationId(value); _locationId = value; } }

        private int _contactId;
        public int ContactId { get { return _contactId; } set { ValidateContactId(value); _contactId = value; } }

        private int _isActive = 1;
        public int IsActive { get { return _isActive; } set { ValidateIsActive(value); _isActive = value; } }

        public Cuisine Cuisine { get; set; }

        public Location Location { get; set; }

        public ContactInformation ContactInformation { get; set; }

        private void ValidateRestaurantId(int value)
        {
            if (value <= 0)
            {
                throw new RestaurantException("Invalid RestaurantId");
            }
        }

        private void ValidateName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new RestaurantException("Name is empty");
            }
        }

        private void ValidateCuisineId(int value)
        {
            if (value <= 0)
            {
                throw new RestaurantException("Invalid CuisineId");
            }
        }

        private void ValidateLocationId(int value)
        {
            if (value <= 0)
            {
                throw new RestaurantException("Invalid LocationId");
            }
        }

        private void ValidateContactId(int value)
        {
            if (value <= 0)
            {
                throw new RestaurantException("Invalid ContactId");
            }
        }

        private void ValidateIsActive(int value)
        {
            // Add specific validation if needed
        }
    }
}
