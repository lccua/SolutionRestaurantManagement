using RestaurantManagement.CONSOLE.Model.Input;
using RestaurantManagement.CONSOLE.Model.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Service
{
    public class CustomerServiceClient
    {

        private static readonly HttpClient client = new HttpClient();

        static CustomerServiceClient()
        {
            // Set the base address and default headers only once
            client.BaseAddress = new Uri("https://localhost:7229/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Uri> AddCustomerAysnc(CustomerInputUI customer)
        {

            string path = $"api/Customer";

            HttpResponseMessage response = await client.PostAsJsonAsync(path, customer);

            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

    }
}
