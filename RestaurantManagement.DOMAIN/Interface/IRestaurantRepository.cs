﻿using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DOMAIN.Interface
{
    public interface IRestaurantRepository
    {
        Task<List<Table>> GetAvailableTablesAsync(DateTime reservationDate, TimeSpan reservationHour, int restaurantId);
        Task<List<Restaurant>> GetRestaurantsAsync(int? postalCode, int? cuisneId);
        Task<List<Restaurant>> GetRestaurantsAsync(DateTime date, int amountOfSeats, int? postalCode, int? cuisineId);
        Task<Restaurant> GetRestaurantAsync(int restaurantId);
        Task<int> AddRestaurantAsync(Restaurant restaurant); 
        Task DeleteRestaurantAsync(int restaurantId);
        Task<bool> IsValidRestaurantAsync(int restaurantId);
        Task UpdateRestaurantAsync(int restaurantId, Restaurant restaurant);


    }
}
