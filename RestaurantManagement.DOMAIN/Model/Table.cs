using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Exception.Model;


namespace RestaurantManagement.DOMAIN.Model
{
    public class Table
    {
        private int _tableNumber;
        public int TableNumber { get { return _tableNumber; } set { ValidateTableNumber(value); _tableNumber = value; } }

        private int _capacity;
        public int Capacity { get { return _capacity; } set { ValidateCapacity(value); _capacity = value; } }

        private void ValidateTableNumber(int value)
        {
            if (value <= 0)
            {
                throw new TableException("Invalid ValidateTableNumber");
            }
        }

        private void ValidateCapacity(int value)
        {
            if (value <= 0)
            {
                throw new TableException("Capacity is empty");
            }
        }
    }
}
