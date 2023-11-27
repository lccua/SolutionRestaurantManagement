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

    }
}
