using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagement.DOMAIN.Interface;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.DATA.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private string _connectionString;

        public RestaurantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Table>> GetAvailableTablesAsync(DateTime date, TimeSpan hour, int restaurantId)
        {
            List<Table> availableTables = new List<Table>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sqlQuery = @"
                        SELECT T.TableNumber, T.Capacity
                        FROM [Table] T
                        WHERE T.RestaurantId = @RestaurantId
                            AND T.TableNumber NOT IN (
                                SELECT R.TableNumber
                                FROM Reservation R
                                WHERE R.RestaurantId = @RestaurantId
                                    AND R.ReservationDate = @ReservationDate
                                    AND (@ReservationHour >= R.StartHour AND @ReservationHour < R.EndHour)
                            );
                    ";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@ReservationDate", date);
                        command.Parameters.AddWithValue("@ReservationHour", hour);
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Table table = new Table
                                {
                                    TableNumber = (int)reader["TableNumber"],
                                    Capacity = (int)reader["Capacity"]
                                };

                                availableTables.Add(table);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return availableTables;
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync(int? postalCode, int? cuisineId)
        {
            List<Restaurant> restaurants = new List<Restaurant>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                  /*  if (postalCode == null) { }

                    else if (cuisineId == null) { }

                    else
                    {
                        throw new Exception("both postalCode and cuisineId where NULL");
                    }*/

                    string sqlQuery = @"
                                        SELECT
                                            R.RestaurantId,
                                            R.Name AS RestaurantName,
                                            L.PostalCode,
                                            L.MunicipalityName,
                                            L.StreetName,
                                            L.HouseNumber,
                                            C.CuisineType,
                                            CI.Email,
                                            CI.PhoneNumber
                                        FROM
                                            Restaurant AS R
                                        JOIN
                                            Location AS L ON R.LocationId = L.LocationId
                                        JOIN
                                            Cuisine AS C ON R.CuisineId = C.CuisineId
                                        JOIN
                                            ContactInformation AS CI ON R.ContactInformationId = CI.ContactInformationId
                                        WHERE
                                            (L.PostalCode = @PostalCode OR @PostalCode IS NULL)
                                            AND
                                            (C.CuisineId = @CuisineId OR @CuisineId IS NULL);
                                    ";


                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@PostalCode", postalCode ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CuisineId", cuisineId ?? (object)DBNull.Value);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Restaurant restaurant = new Restaurant
                                {
                                    Name = reader.GetString(reader.GetOrdinal("RestaurantName")),

                                    Location = new Location
                                    {
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("PostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("MunicipalityName")),
                                        StreetName = reader.GetString(reader.GetOrdinal("StreetName")),
                                        HouseNumber = reader.GetString(reader.GetOrdinal("HouseNumber")),
                                    },
                                    Cuisine = new Cuisine
                                    {
                                        CuisineType = reader.GetString(reader.GetOrdinal("CuisineType")),
                                    },
                                    ContactInformation = new ContactInformation
                                    {
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    }
                                   
                                };

                                restaurants.Add(restaurant);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception or handle it appropriately based on your application's logging strategy
            }

            return restaurants;
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync(DateTime date, int amountOfSeats, int? postalCode, int? cuisineId)
        {
            List<Restaurant> restaurants = new List<Restaurant>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sqlQuery = @"
                                        SELECT DISTINCT
                                            R.RestaurantId,
                                            R.Name AS RestaurantName,
                                            L.PostalCode,
                                            L.MunicipalityName,
                                            L.StreetName,
                                            L.HouseNumber,
                                            C.CuisineType,
                                            CI.Email,
                                            CI.PhoneNumber
                                        FROM
                                            Restaurant AS R
                                        JOIN
                                            Location AS L ON R.LocationId = L.LocationId
                                        JOIN
                                            Cuisine AS C ON R.CuisineId = C.CuisineId
                                        JOIN
                                            ContactInformation AS CI ON R.ContactInformationId = CI.ContactInformationId
                                        JOIN
                                            [Table] AS T ON R.RestaurantId = T.RestaurantId
                                        LEFT JOIN
                                            Reservation AS Res ON R.RestaurantId = Res.RestaurantId AND Res.ReservationDate = @ReservationDate
                                        WHERE
                                            (L.PostalCode = @PostalCode OR @PostalCode IS NULL)
                                            AND
                                            (C.CuisineId = @CuisineId OR @CuisineId IS NULL)
                                            AND
                                            T.Capacity >= @AmountOfSeats
                                            AND
                                            R.IsActive = 1
                                            AND
                                            (Res.ReservationNumber IS NULL OR Res.AmountOfSeats + @AmountOfSeats <= T.Capacity);

                                        ";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@PostalCode", postalCode ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CuisineId", cuisineId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ReservationDate", date);
                        command.Parameters.AddWithValue("@AmountOfSeats", amountOfSeats);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Restaurant restaurant = new Restaurant
                                {
                                    Name = reader.GetString(reader.GetOrdinal("RestaurantName")),
                                    Location = new Location
                                    {
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("PostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("MunicipalityName")),
                                        StreetName = reader.GetString(reader.GetOrdinal("StreetName")),
                                        HouseNumber = reader.GetString(reader.GetOrdinal("HouseNumber")),
                                    },
                                    Cuisine = new Cuisine
                                    {
                                        CuisineType = reader.GetString(reader.GetOrdinal("CuisineType")),
                                    },
                                    ContactInformation = new ContactInformation
                                    {
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    }
                                };

                                restaurants.Add(restaurant);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the exception or handle it appropriately based on your application's logging strategy
            }

            return restaurants;
        }






    }
}
