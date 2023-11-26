using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Exception.Model;

namespace RestaurantManagement.DOMAIN.Model
{
    public class Admin
    {
        private int _id;
        public int Id { get { return _id; } set { ValidateId(value); _id = value; } }

        private string _name;
        public string Name { get { return _name; } set { ValidateName(value); _name = value; } }

        private int _contactInfoId;
        public int ContactInfoId { get { return _contactInfoId; } set { ValidateContactInfoId(value); _contactInfoId = value; } }

        private int _isActive = 1;
        public int IsActive { get { return _isActive; } set { ValidateIsActive(value); _isActive = value; } }

        public ContactInformation ContactInformation { get; set; }

        private void ValidateId(int value)
        {
            if (value <= 0)
            {
                throw new AdminException("Invalid ID");
            }
        }

        private void ValidateName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new AdminException("Name is empty");
            }
        }

        private void ValidateContactInfoId(int value)
        {
            // Add specific validation if needed
        }

        private void ValidateIsActive(int value)
        {
            // Add specific validation if needed
        }
    }
}
