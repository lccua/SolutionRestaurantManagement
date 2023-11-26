using RestaurantManagement.DOMAIN.Interface;
using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DATA.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private string _connectionString;

        public ReservationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Start a transaction
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        // Associate the command with the transaction
                        command.Transaction = transaction;

                        command.CommandText = "INSERT INTO Reservation (AmountOfSeats, Date, StartHour, EndHour, TableNumber, CustomerNumber, RestaurantId) VALUES (@AmountOfSeats, @Date, @StartHour, @EndHour, @TableNumber, @CustomerNumber, @RestaurantId)";

                        // Add parameters
                        command.Parameters.AddWithValue("@AmountOfSeats", reservation.AmountOfSeats);
                        command.Parameters.AddWithValue("@Date", reservation.Date);
                        command.Parameters.AddWithValue("@StartHour", reservation.StartHour);
                        command.Parameters.AddWithValue("@EndHour", reservation.EndHour);
                        command.Parameters.AddWithValue("@RestaurantId", reservation.RestaurantId);
                        command.Parameters.AddWithValue("@TableNumber", reservation.TableNumber);
                        command.Parameters.AddWithValue("@CustomerNumber", reservation.CustomerNumber);

                        Console.WriteLine($"Executing SQL command: {command.CommandText}");


                        await command.ExecuteNonQueryAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Console.WriteLine(ex);

                    throw;
                }
            }
        }

        public async Task<List<Table>> GetAvailableTables(DateTime date, TimeSpan hour, int restaurantId)
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
