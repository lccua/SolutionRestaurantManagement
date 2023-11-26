using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DOMAIN.Interface
{
    public interface ICustomerRepository
    {
        Task RegisterCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);
        Task<Customer> GetCustomerAsync(int customerNumber);
        Task UpdateCustomerAsync(Customer customer);
    }
}
