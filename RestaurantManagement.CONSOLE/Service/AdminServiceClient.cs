﻿using RestaurantManagement.CONSOLE.Model.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantManagement.CONSOLE.Service
{
    public class AdminServiceClient
    {
        private static readonly HttpClient client = new HttpClient();

        static AdminServiceClient()
        {
            // Set the base address and default headers only once
            client.BaseAddress = new Uri("https://localhost:7229/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<ReservationOutputUI>> GetRestaurantReservationsByDayAsync(int restaurantId, string date)
        {
            try 
            {
                string path = $"api/Admin/Restaurant/{restaurantId}/Reservations/Day?date={date}";

                HttpResponseMessage response = await client.GetAsync(path);

                var responseContent = response.Content.ReadAsStringAsync().Result;


                List<ReservationOutputUI> restaurantReservationResponse = null;

                if (response.IsSuccessStatusCode)
                {
                    restaurantReservationResponse = JsonSerializer.Deserialize<List<ReservationOutputUI>>(responseContent);
                }

                return restaurantReservationResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
                return null;
            }
            
        }
    }
}
