using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Output
{
    public class ContactInformationOutputUI
    {
        public int id { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }

        public override string ToString()
        {
            return $"{email}, {phoneNumber}";
        }
    }
}
