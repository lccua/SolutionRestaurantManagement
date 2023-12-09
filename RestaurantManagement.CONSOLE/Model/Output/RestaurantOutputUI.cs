using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Output
{
    public class RestaurantOutputUI
    {
        public int restaurantId { get; set; }
        public string restaurantName { get; set; }
        public CuisineOutputUI cuisineOutput { get; set; }
        public ContactInformationOutputUI contactInformationOutput { get; set; }
        public LocationOutputUI locationOutput { get; set; }

        public override string ToString()
        {
            return $"{restaurantName} (ID: {restaurantId}), Cuisine: {cuisineOutput}, Contact: {contactInformationOutput}, Location: {locationOutput}";
        }
    }
}
