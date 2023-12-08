using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RestaurantManagement.PRESENTATION.Model;

namespace RestaurantManagement.PRESENTATION.Service
{
    

    public class AdminServiceClient
    {
        private static readonly HttpClient client = new HttpClient();

        static AdminServiceClient()
        {
            // Set the base address and default headers only once
            client.BaseAddress = new Uri("http://localhost:5044/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<ReservationOutputUI>> GetRestaurantReservationsByDay(int reeksId, string date)
        {
            try
            {
                string path = $"api/RestaurantManagement/Restaurant/GetByDay/{date}"; // Construct the path using the provided ID
                HttpResponseMessage response = await client.GetAsync(path);

                var responseContent = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine("RESULT CreateAuteurAsync");
                Console.WriteLine(responseContent);

                ReservationOutputUI reeksResponse = null;

                if (response.IsSuccessStatusCode)
                {
                    reeksResponse = JsonSerializer.Deserialize<ReservationOutputUI>(responseContent);
                }

                return reeksResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
                return null;
            }
        }
    }
}
