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

                        command.CommandText = "INSERT INTO Reservation (AmountOfSeats, ReservationDate, StartHour, EndHour, TableNumber, CustomerNumber, RestaurantId) VALUES (@AmountOfSeats, @ReservationDate, @StartHour, @EndHour, @TableNumber, @CustomerNumber, @RestaurantId)";

                        // Add parameters
                        command.Parameters.AddWithValue("@AmountOfSeats", reservation.AmountOfSeats);
                        command.Parameters.AddWithValue("@ReservationDate", reservation.Date);
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


        public async Task<bool> IsValidReservationAsync(int reservationId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = @"
                SELECT 1
                FROM Reservation
                WHERE ReservationNumber = @ReservationId
                AND ReservationDate >= GETDATE()";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservationId);

                    var result = await command.ExecuteScalarAsync();
                    return result != null && (int)result == 1;
                }
            }
        }

        public async Task UpdateReservationAsync(int reservationNumber, Reservation reservation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Update ContactInformation
                            string updateContactQuery = @"
                                                        UPDATE Reservation 
                                                        SET 
                                                            ReservationDate = @ReservationDate,
                                                            StartHour = @StartHour,
                                                            AmountOfSeats = @AmountOfSeats
                                                        WHERE ReservationNumber = @ReservationNumber";

                            using (SqlCommand contactCommand = new SqlCommand(updateContactQuery, connection, transaction))
                            {
                                contactCommand.Parameters.AddWithValue("@ReservationDate", reservation.Date);
                                contactCommand.Parameters.AddWithValue("@StartHour", reservation.StartHour);
                                contactCommand.Parameters.AddWithValue("@AmountOfSeats", reservation.AmountOfSeats);
                                contactCommand.Parameters.AddWithValue("@ReservationNumber", reservationNumber);



                                await contactCommand.ExecuteNonQueryAsync();
                            }

                          
                            // If everything is successful, commit the transaction
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // An error occurred, rollback the transaction
                            transaction.Rollback();

                            Console.WriteLine($"Error in UpdateReservationAsync: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateReservationAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Reservation> GetReservationAsync(int reservationNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                                    SELECT 
                                        r.ReservationNumber
                                    FROM Reservation r
                                    WHERE r.ReservationNumber = @ReservationNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationNumber", reservationNumber);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                Reservation reservation = new Reservation
                                {
                                    ReservationNumber = (int)reader["ReservationNumber"], 
                                };

                                return reservation;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, throw, etc.)
                // You may want to customize this based on your application's error handling strategy
                Console.WriteLine($"Error in GetReservationAsync: {ex.Message}");
            }

            return null; // Reservation not found or an error occurred
        }



    }
}
