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

    }
}
