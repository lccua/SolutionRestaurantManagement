using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Interface;

namespace RestaurantManagement.DATA.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private string _connectionString;

        public RestaurantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
