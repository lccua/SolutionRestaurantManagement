using System;
using System.Collections.Generic;
using System.Data;
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


        public async Task<Restaurant> GetRestaurantAsync(int restaurantId)
        {
            Restaurant restaurant = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Begin a transaction using the 'using' statement
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                                        SELECT 
                                            R.RestaurantId,
                                            R.Name,
                                            R.CuisineId,
                                            R.LocationId,
                                            R.ContactInformationId,
                                            CI.Email,
                                            CI.PhoneNumber,
                                            L.LocationId,
                                            L.PostalCode,
                                            L.MunicipalityName,
                                            L.StreetName,
                                            L.HouseNumber,
                                            C.CuisineId,
                                            C.CuisineType
                                        FROM 
                                            Restaurant R
                                        INNER JOIN 
                                            ContactInformation CI ON R.ContactInformationId = CI.ContactInformationId
                                        INNER JOIN 
                                            Location L ON R.LocationId = L.LocationId
                                        INNER JOIN 
                                            Cuisine C ON R.CuisineId = C.CuisineId
                                        WHERE 
                                            R.RestaurantId = @RestaurantId";

                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@RestaurantId", restaurantId);

                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    ContactInformation contactInformation = new ContactInformation
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ContactInformationId")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    };

                                    Location location = new Location
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("LocationId")),
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("PostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("MunicipalityName")),
                                        StreetName = reader.GetString(reader.GetOrdinal("StreetName")),
                                        HouseNumber = reader.GetString(reader.GetOrdinal("HouseNumber")),
                                    };

                                    Cuisine cuisine = new Cuisine
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CuisineId")),
                                        CuisineType = reader.GetString(reader.GetOrdinal("CuisineType")),
                                    };

                                    restaurant = new Restaurant
                                    {
                                        RestaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantId")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        ContactInformation = contactInformation,
                                        Location = location,
                                        Cuisine = cuisine,
                                    };
                                }
                            }
                        }

                        // The 'using' statement will automatically commit the transaction if no exceptions occur
                    }
                    catch (Exception)
                    {
                        // The 'using' statement will automatically roll back the transaction in case of an exception
                        throw; // Rethrow the exception for handling at a higher level
                    }
                }
            }

            return restaurant;
        }

        public async Task<int> AddRestaurantAsync(Restaurant restaurant)
        {
            try
            {
                string insertCuisineSQL = "INSERT INTO Cuisine(CuisineType) OUTPUT INSERTED.CuisineId VALUES(@cuisineType)";
                string insertLocationSQL = "INSERT INTO Location(PostalCode, MunicipalityName, StreetName, HouseNumber) OUTPUT INSERTED.LocationId VALUES(@postalCode, @municipalityName, @streetName, @houseNumber)";
                string insertContactInformationSQL = "INSERT INTO ContactInformation(Email, PhoneNumber) OUTPUT INSERTED.ContactInformationId VALUES(@email, @phoneNumber)";
                string insertRestaurantSQL = "INSERT INTO Restaurant(Name, CuisineId, LocationId, ContactInformationId) OUTPUT INSERTED.RestaurantId VALUES(@name, @cuisineId, @locationId, @contactInformationId)";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert Cuisine
                        command.CommandText = insertCuisineSQL;
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@cuisineType", restaurant.Cuisine.CuisineType);
                        int cuisineId = (int)await command.ExecuteScalarAsync();

                        // Insert Location
                        command.CommandText = insertLocationSQL;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@postalCode", restaurant.Location.PostalCode);
                        command.Parameters.AddWithValue("@municipalityName", restaurant.Location.MunicipalityName);
                        command.Parameters.AddWithValue("@streetName", restaurant.Location.StreetName);
                        command.Parameters.AddWithValue("@houseNumber", restaurant.Location.HouseNumber);
                        int locationId = (int)await command.ExecuteScalarAsync();

                        // Insert ContactInformation
                        command.CommandText = insertContactInformationSQL;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@email", restaurant.ContactInformation.Email);
                        command.Parameters.AddWithValue("@phoneNumber", restaurant.ContactInformation.PhoneNumber);
                        int contactInformationId = (int)await command.ExecuteScalarAsync();

                        // Insert Restaurant
                        command.CommandText = insertRestaurantSQL;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@name", restaurant.Name);
                        command.Parameters.AddWithValue("@cuisineId", cuisineId);
                        command.Parameters.AddWithValue("@locationId", locationId);
                        command.Parameters.AddWithValue("@contactInformationId", contactInformationId);

                        // Retrieve the newly inserted RestaurantId
                        int restaurantId = (int)await command.ExecuteScalarAsync();

                        transaction.Commit();

                        // Return the RestaurantId
                        return restaurantId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding restaurant.", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding restaurant", ex);
            }
        }

        public async Task UpdateRestaurantAsync(int restaurantId, Restaurant restaurant)
        {
            try
            {
                // Retrieve existing LocationId, ContactInformationId, and CuisineId
                int existingLocationId;
                int existingContactInformationId;
                int existingCuisineId;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();

                    // Retrieve existing LocationId, ContactInformationId, and CuisineId from the Restaurant table
                    string selectIdsSQL = "SELECT LocationId, ContactInformationId, CuisineId FROM Restaurant WHERE RestaurantId = @restaurantId";

                    command.CommandText = selectIdsSQL;
                    command.Parameters.AddWithValue("@restaurantId", restaurantId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            existingLocationId = reader.GetInt32(0);
                            existingContactInformationId = reader.GetInt32(1);
                            existingCuisineId = reader.GetInt32(2);
                        }
                        else
                        {
                            throw new Exception($"Restaurant with ID {restaurantId} not found.");
                        }
                    }
                }

                // Now you can use existingLocationId, existingContactInformationId, and existingCuisineId as needed

                // Proceed with your update logic here...

                string updateCuisineSQL = "UPDATE Cuisine SET CuisineType = @cuisineType WHERE CuisineId = @cuisineId";
                string updateLocationSQL = "UPDATE Location SET PostalCode = @postalCode, MunicipalityName = @municipalityName, StreetName = @streetName, HouseNumber = @houseNumber WHERE LocationId = @locationId";
                string updateContactInformationSQL = "UPDATE ContactInformation SET Email = @email, PhoneNumber = @phoneNumber WHERE ContactInformationId = @contactInformationId";
                string updateRestaurantSQL = "UPDATE Restaurant SET Name = @name WHERE RestaurantId = @restaurantId";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Update Cuisine
                        command.CommandText = updateCuisineSQL;
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@cuisineType", restaurant.Cuisine.CuisineType);
                        command.Parameters.AddWithValue("@cuisineId", existingCuisineId); // Use existingCuisineId
                        await command.ExecuteNonQueryAsync();

                        // Update Location
                        command.CommandText = updateLocationSQL;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@postalCode", restaurant.Location.PostalCode);
                        command.Parameters.AddWithValue("@municipalityName", restaurant.Location.MunicipalityName);
                        command.Parameters.AddWithValue("@streetName", restaurant.Location.StreetName);
                        command.Parameters.AddWithValue("@houseNumber", restaurant.Location.HouseNumber);
                        command.Parameters.AddWithValue("@locationId", existingLocationId); // Use existingLocationId
                        await command.ExecuteNonQueryAsync();

                        // Update ContactInformation
                        command.CommandText = updateContactInformationSQL;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@email", restaurant.ContactInformation.Email);
                        command.Parameters.AddWithValue("@phoneNumber", restaurant.ContactInformation.PhoneNumber);
                        command.Parameters.AddWithValue("@contactInformationId", existingContactInformationId); // Use existingContactInformationId
                        await command.ExecuteNonQueryAsync();

                        // Update Restaurant
                        command.CommandText = updateRestaurantSQL;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@name", restaurant.Name);
                        command.Parameters.AddWithValue("@restaurantId", restaurantId);
                        await command.ExecuteNonQueryAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating restaurant.", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating restaurant", ex);
            }
        }


        public async Task DeleteRestaurantAsync(int restaurantId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Restaurant SET IsActive = 0 WHERE RestaurantId = @RestaurantId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RestaurantId", restaurantId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


    }
}
