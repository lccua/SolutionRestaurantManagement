using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantManagement.PRESENTATION.Service
{
    public class StripServiceClient
    {
        private static readonly HttpClient client = new HttpClient();

        static StripServiceClient()
        {
            // Set the base address and default headers only once
            client.BaseAddress = new Uri("http://localhost:5044/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ReeksOutputUI> GetReeksAsync(int reeksId)
        {
            try
            {
                string path = $"api/Strips/beheer/Reeks/{reeksId}"; // Construct the path using the provided ID
                HttpResponseMessage response = await client.GetAsync(path);

                var responseContent = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine("RESULT CreateAuteurAsync");
                Console.WriteLine(responseContent);

                ReeksOutputUI reeksResponse = null;

                if (response.IsSuccessStatusCode)
                {
                    reeksResponse = JsonSerializer.Deserialize<ReeksOutputUI>(responseContent);
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
