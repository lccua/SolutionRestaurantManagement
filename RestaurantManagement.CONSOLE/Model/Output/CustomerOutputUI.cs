using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Output
{
    public class CustomerOutputUI
    {
        public int customerNumber { get; set; }
        public string name { get; set; }
        public LocationOutputUI locationOutput { get; set; }
        public ContactInformationOutputUI contactInformationOutput { get; set; }

        public override string ToString()
        {
            return $"{name} (Customer #{customerNumber}), Contact: {contactInformationOutput}, Location: {locationOutput}";
        }
    }
}
