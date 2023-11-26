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

                        command.CommandText = "INSERT INTO Reservation (AmountOfSeats, Date, Hour, TableNumber, CustomerNumber, RestaurantId) VALUES (@AmountOfSeats, @Date, @Hour, @TableNumber, @CustomerNumber, @RestaurantId)";

                        // Add parameters
                        command.Parameters.AddWithValue("@AmountOfSeats", reservation.AmountOfSeats);
                        command.Parameters.AddWithValue("@Date", reservation.Date);
                        command.Parameters.AddWithValue("@Hour", reservation.Hour);
                        command.Parameters.AddWithValue("@TableNumber", reservation.TableNumber);
                        command.Parameters.AddWithValue("@CustomerNumber", reservation.CustomerNumber);
                        command.Parameters.AddWithValue("@RestaurantId", reservation.RestaurantId);

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
