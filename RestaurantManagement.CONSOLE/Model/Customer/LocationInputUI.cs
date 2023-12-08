using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Customer
{
    public class LocationInputUI
    {
        public int PostalCode { get; set; }
        public string MunicipalityName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
    }
}
