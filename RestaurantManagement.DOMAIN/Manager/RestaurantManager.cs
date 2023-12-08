using RestaurantManagement.DOMAIN.Interface;
using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DOMAIN.Manager
{
    public class RestaurantManager
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantManager(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<List<Table>> GetAvailableTablesAsync(DateTime reservationDate, TimeSpan reservationHour, int restaurantId)
        {
            try
            {
                return await _restaurantRepository.GetAvailableTablesAsync(reservationDate, reservationHour, restaurantId);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync(int? postalCode, int? cuisneId)
        {
            try
            {
                return await _restaurantRepository.GetRestaurantsAsync(postalCode, cuisneId);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        
        public async Task<List<Restaurant>> GetRestaurantsAsync(DateTime date, int amountOfSeats, int? postalCode, int? cuisineId)
        {
            try
            {
                return await _restaurantRepository.GetRestaurantsAsync(date, amountOfSeats, postalCode, cuisineId);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<Restaurant> GetRestaurantAsync(int restaurantId)
        {
            try
            {
                return await _restaurantRepository.GetRestaurantAsync(restaurantId);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);

                throw;
            }
        }

        public async Task<int> AddRestaurantAsync(Restaurant restaurant)
        {
            try
            {
                return await _restaurantRepository.AddRestaurantAsync(restaurant);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);

                throw;
            }
        }

        public async Task DeleteRestaurantAsync(int restaurantId)
        {
            try
            {
                await _restaurantRepository.DeleteRestaurantAsync(restaurantId);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<bool> IsValidRestaurantAsync(int restaurantId)
        {
            try
            {
                return await _restaurantRepository.IsValidRestaurantAsync(restaurantId);

            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
