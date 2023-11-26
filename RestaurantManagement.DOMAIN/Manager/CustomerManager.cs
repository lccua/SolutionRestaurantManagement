using RestaurantManagement.DOMAIN.Interface;
using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DOMAIN.Manager
{
    public class CustomerManager
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task RegisterCustomerAsync(Customer customer)
        {
            try
            {
                await _customerRepository.RegisterCustomerAsync(customer);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            try
            {
                return await _customerRepository.GetCustomerAsync(customerId);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            try
            {
                await _customerRepository.DeleteCustomerAsync(customerId);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            try
            {
                await _customerRepository.UpdateCustomerAsync(customer);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
