using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Input
{
    public class CustomerInputUI
    {
        public string Name { get; set; }
        public ContactInformationInputUI ContactInformationInput { get; set; }
        public LocationInputUI LocationInput { get; set; }
    }
}
