using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Exception.Model;


namespace RestaurantManagement.DOMAIN.Model
{
    public class Cuisine
    {
        private int _id;
        public int Id { get { return _id; } set { ValidateId(value); _id = value; } }

        private string _cuisineType;
        public string CuisineType { get { return _cuisineType; } set { ValidateCuisineType(value); _cuisineType = value; } }

        private void ValidateId(int value)
        {
            if (value <= 0)
            {
                throw new CuisineException("Invalid ID");
            }
        }

        private void ValidateCuisineType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new CuisineException("CuisineType is empty");
            }
        }
    }
}
