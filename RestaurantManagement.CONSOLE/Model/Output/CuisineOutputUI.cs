using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Model.Output
{
    public class CuisineOutputUI
    {
        public int cuisineId { get; set; }
        public string cuisineType { get; set; }

        public override string ToString()
        {
            return $"{cuisineType} (ID: {cuisineId})";
        }
    }
}
