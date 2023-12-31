﻿using RestaurantManagement.DOMAIN.Interface;
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

        public async Task<int> RegisterCustomerAsync(Customer customer)
        {
            try
            {
                return await _customerRepository.RegisterCustomerAsync(customer);
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

        public async Task UpdateCustomerAsync(int customerNumber, Customer customer)
        {
            try
            {
                await _customerRepository.UpdateCustomerAsync(customerNumber, customer);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<bool> IsValidCustomerAsync(int customerId)
        {
            try
            {
                return await _customerRepository.IsValidCustomerAsync(customerId);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
