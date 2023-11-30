using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Exception.Model;

namespace RestaurantManagement.DOMAIN.Model
{
    public class ContactInformation
    {
       

        private int _id;
        public int Id { get { return _id; } set { ValidateId(value); _id = value; } }

        private string _email;
        public string Email { get { return _email; } set { ValidateEmail(value); _email = value; } }

        private string _phoneNumber;
        public string PhoneNumber { get { return _phoneNumber; } set { ValidatePhoneNumber(value); _phoneNumber = value; } }

        private void ValidateId(int value)
        {
            if (value <= 0)
            {
                throw new ContactInformationException("Invalid ID");
            }
        }

        private void ValidateEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ContactInformationException("Email is empty");
            }
        }

        private void ValidatePhoneNumber(string value)
        {
            if (!IsNumeric(value))
            {
                throw new ContactInformationException("Invalid phone number. Must contain only numeric characters.");
            }
        }

        private bool IsNumeric(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
