using RestaurantManagement.CONSOLE.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Admin
{
    public class ReservationOutputUI
    {
        public string Name { get; set; }
        public CuisineOutputUI CuisineOutput {  get; set; }
        public LocationOutputUI LocationOutput { get; set; }
        public ContactInformationOutputUI ContactInformationOutput { get; set; }
    }
}
