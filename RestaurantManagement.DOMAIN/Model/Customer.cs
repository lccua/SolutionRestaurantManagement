using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Exception.Model;

namespace RestaurantManagement.DOMAIN.Model
{
    public class Customer
    {

        public Customer()
        {

        }

        public Customer(int customerNumber, string name, ContactInformation contactInformation, Location location)
        {
            _customerNumber = customerNumber;
            _name = name;
            _contactInformation = contactInformation;
            _location = location;
        }

        private int _customerNumber;
        public int CustomerNumber { get { return _customerNumber; } set { ValidateCustomerNumber(value); _customerNumber = value; } }

        private string _name;
        public string Name { get { return _name; } set { ValidateName(value); _name = value; } }

        private int _isActive = 1;
        public int IsActive { get { return _isActive; } set { ValidateIsActive(value); _isActive = value; } }

        private ContactInformation _contactInformation;
        public ContactInformation ContactInformation { get { return _contactInformation; } set { ValidateContactInformation(value); _contactInformation = value; } }

        private Location _location;
        public Location Location { get { return _location; } set { ValidateLocation(value); _location = value; } }


        private void ValidateCustomerNumber(int value)
        {
            if (value <= 0)
            {
                throw new CustomerException("Invalid CustomerNumber");
            }
        }

        private void ValidateName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new CustomerException("Name is empty");
            }
        }

        private void ValidateContactInformation(ContactInformation contactInformation)
        {
            if (contactInformation == null)
            {
                throw new CustomerException("ContactInformation cannot be null");
            }
            // You can add more specific validation for ContactInformation if needed
        }

        private void ValidateLocation(Location location)
        {
            if (location == null)
            {
                throw new CustomerException("Location cannot be null");
            }
            // You can add more specific validation for Location if needed
        }

        private void ValidateIsActive(int value)
        {
            if (value <= 0)
            {
                throw new CustomerException("Invalid IsActive");
            }
        }
    }
}
