using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Output
{
    public class LocationOutputUI
    {
        public int id { get; set; }
        public int postalCode { get; set; }
        public string municipalityName { get; set; }
        public string streetName { get; set; }
        public string houseNumber { get; set; }

        public override string ToString()
        {
            return $"{streetName} {houseNumber}, {municipalityName}, {postalCode}";
        }
    }
}
