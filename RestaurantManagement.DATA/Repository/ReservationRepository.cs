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

        public async Task CancelReservationAsync(int reservationNumber)
        {
            // Define your SQL query to delete the reservation with a condition for ReservationDate
            string deleteQuery = $"DELETE FROM Reservation WHERE ReservationNumber = @ReservationNumber AND ReservationDate >= @Today";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Your SQL command to delete the reservation with parameters
                        using (var command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ReservationNumber", reservationNumber);
                            command.Parameters.AddWithValue("@Today", DateTime.Today);

                            command.Transaction = transaction;

                            // Execute the DELETE command
                            await command.ExecuteNonQueryAsync();

                            // Commit the transaction if the deletion is successful
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        transaction.Rollback();

                        throw;
                    }
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
                            // Check for overlapping reservations
                            if (await CheckOverlappingReservationsAsync(connection, transaction, reservationNumber, reservation))
                            {
                                throw new InvalidOperationException("Overlapping reservations found.");
                            }

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

        private async Task<bool> CheckOverlappingReservationsAsync(SqlConnection connection, SqlTransaction transaction, int reservationNumber, Reservation reservation)
        {
            // Retrieve existing values for EndHour and TableNumber
            string retrieveExistingValuesQuery = @"
                                                    SELECT EndHour, TableNumber
                                                    FROM Reservation
                                                    WHERE ReservationNumber = @ReservationNumber";

            using (SqlCommand retrieveCommand = new SqlCommand(retrieveExistingValuesQuery, connection, transaction))
            {
                retrieveCommand.Parameters.AddWithValue("@ReservationNumber", reservationNumber);

                using (var reader = await retrieveCommand.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        reservation.EndHour = reader.GetTimeSpan(reader.GetOrdinal("EndHour"));
                        reservation.TableNumber = reader.GetInt32(reader.GetOrdinal("TableNumber"));
                    }
                    else
                    {
                        // Handle the case where the reservation with the given number is not found
                        throw new InvalidOperationException("Reservation not found.");
                    }
                }
            }

            // Now that you have EndHour and TableNumber, perform the check for overlapping reservations
            string checkOverlapQuery = @"
                                        SELECT 1
                                        FROM Reservation
                                        WHERE ReservationDate = @ReservationDate
                                            AND TableNumber = @TableNumber
                                            AND (
                                                (@StartHour <= EndHour AND @EndHour >= StartHour)
                                                OR (@StartHour >= StartHour AND @EndHour <= EndHour)
                                            )
                                            AND ReservationNumber != @ReservationNumber";

            using (SqlCommand overlapCommand = new SqlCommand(checkOverlapQuery, connection, transaction))
            {
                overlapCommand.Parameters.AddWithValue("@ReservationDate", reservation.Date);
                overlapCommand.Parameters.AddWithValue("@TableNumber", reservation.TableNumber);
                overlapCommand.Parameters.AddWithValue("@StartHour", reservation.StartHour);
                overlapCommand.Parameters.AddWithValue("@EndHour", reservation.EndHour);
                overlapCommand.Parameters.AddWithValue("@ReservationNumber", reservationNumber);

                using (var reader = await overlapCommand.ExecuteReaderAsync())
                {
                    return reader.HasRows;
                }
            }
        }

        public async Task<List<Reservation>> GetCustomerReservationsByPeriodAsync(int customerNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Reservation> reservationList = new List<Reservation>();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                                    SELECT 
                                        r.ReservationNumber,
                                        r.StartHour,
                                        r.EndHour,
                                        r.AmountOfSeats,
                                        r.ReservationDate,
                                        r.CustomerNumber,
                                        r.RestaurantId,
                                        r.TableNumber,
                                        res.Name AS RestaurantName,
                                        res.CuisineId,
                                        res.LocationId AS RestaurantLocationId,
                                        res.ContactInformationId AS RestaurantContactInfoId,
                                        cu.CuisineType,
                                        loc.PostalCode AS RestaurantPostalCode,
                                        loc.MunicipalityName AS RestaurantMunicipality,
                                        loc.StreetName AS RestaurantStreet,
                                        loc.HouseNumber AS RestaurantHouseNumber,
                                        cont.Email AS RestaurantEmail,
                                        cont.PhoneNumber AS RestaurantPhoneNumber,
                                        tab.Capacity,
                                        cus.Name AS CustomerName,
                                        cus.LocationId AS CustomerLocationId,
                                        cus.ContactInformationId AS CustomerContactInfoId,
                                        cloc.PostalCode AS CustomerPostalCode,
                                        cloc.MunicipalityName AS CustomerMunicipality,
                                        cloc.StreetName AS CustomerStreet,
                                        cloc.HouseNumber AS CustomerHouseNumber,
                                        ccont.Email AS CustomerEmail,
                                        ccont.PhoneNumber AS CustomerPhoneNumber
                                    FROM Reservation r
                                    INNER JOIN Restaurant res ON r.RestaurantId = res.RestaurantId
                                    INNER JOIN Cuisine cu ON res.CuisineId = cu.CuisineId
                                    INNER JOIN Location loc ON res.LocationId = loc.LocationId
                                    INNER JOIN ContactInformation cont ON res.ContactInformationId = cont.ContactInformationId
                                    INNER JOIN [Table] tab ON r.TableNumber = tab.TableNumber
                                    INNER JOIN Customer cus ON r.CustomerNumber = cus.CustomerNumber
                                    INNER JOIN Location cloc ON cus.LocationId = cloc.LocationId
                                    INNER JOIN ContactInformation ccont ON cus.ContactInformationId = ccont.ContactInformationId
                                    WHERE r.CustomerNumber = @CustomerNumber
                                          AND r.ReservationDate BETWEEN @StartDate AND @EndDate";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerNumber", customerNumber);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Reservation reservation = new Reservation
                                {
                                    ReservationNumber = reader.GetInt32(reader.GetOrdinal("ReservationNumber")),
                                    StartHour = reader.GetTimeSpan(reader.GetOrdinal("StartHour")),
                                    EndHour = reader.GetTimeSpan(reader.GetOrdinal("EndHour")),
                                    AmountOfSeats = reader.GetInt32(reader.GetOrdinal("AmountOfSeats")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                    CustomerNumber = reader.GetInt32(reader.GetOrdinal("CustomerNumber")),
                                    RestaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantId")),
                                    TableNumber = reader.GetInt32(reader.GetOrdinal("TableNumber"))
                                };

                                reservation.Customer = new Customer
                                {
                                    CustomerNumber = reservation.CustomerNumber,
                                    Name = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    Location = new Location
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerLocationId")),
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("CustomerPostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("CustomerMunicipality")),
                                        StreetName = reader.IsDBNull(reader.GetOrdinal("CustomerStreet")) ? null : reader.GetString(reader.GetOrdinal("CustomerStreet")),
                                        HouseNumber = reader.IsDBNull(reader.GetOrdinal("CustomerHouseNumber")) ? null : reader.GetString(reader.GetOrdinal("CustomerHouseNumber")),
                                    },
                                    ContactInformation = new ContactInformation
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerContactInfoId")),
                                        Email = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("CustomerPhoneNumber")),
                                    }
                                };

                                reservation.Restaurant = new Restaurant
                                {
                                    RestaurantId = reservation.RestaurantId,
                                    Name = reader.GetString(reader.GetOrdinal("RestaurantName")),
                                    Cuisine = new Cuisine
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CuisineId")),
                                        CuisineType = reader.GetString(reader.GetOrdinal("CuisineType")),
                                    },
                                    Location = new Location
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("RestaurantLocationId")),
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("RestaurantPostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("RestaurantMunicipality")),
                                        StreetName = reader.IsDBNull(reader.GetOrdinal("RestaurantStreet")) ? null : reader.GetString(reader.GetOrdinal("RestaurantStreet")),
                                        HouseNumber = reader.IsDBNull(reader.GetOrdinal("RestaurantHouseNumber")) ? null : reader.GetString(reader.GetOrdinal("RestaurantHouseNumber")),
                                    },
                                    ContactInformation = new ContactInformation
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("RestaurantContactInfoId")),
                                        Email = reader.GetString(reader.GetOrdinal("RestaurantEmail")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("RestaurantPhoneNumber")),
                                    }
                                };

                                reservation.Table = new Table
                                {
                                    TableNumber = reservation.TableNumber,
                                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                };

                                reservationList.Add(reservation);
                            }
                        }
                    }
                }

                return reservationList;
            }
            catch (Exception ex)
            {
        
                Console.WriteLine($"Error in GetReservationsAsync: {ex.Message}");
            }

            return null;
        }

        public async Task<List<Reservation>> GetRestaurantReservationsForDayAsync(int restaurantId, DateTime date)
        {
            try
            {
                List<Reservation> reservationList = new List<Reservation>();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                                    SELECT 
                                        r.ReservationNumber,
                                        r.StartHour,
                                        r.EndHour,
                                        r.AmountOfSeats,
                                        r.ReservationDate,
                                        r.CustomerNumber,
                                        r.RestaurantId,
                                        r.TableNumber,
                                        res.Name AS RestaurantName,
                                        res.CuisineId,
                                        res.LocationId AS RestaurantLocationId,
                                        res.ContactInformationId AS RestaurantContactInfoId,
                                        cu.CuisineType,
                                        loc.PostalCode AS RestaurantPostalCode,
                                        loc.MunicipalityName AS RestaurantMunicipality,
                                        loc.StreetName AS RestaurantStreet,
                                        loc.HouseNumber AS RestaurantHouseNumber,
                                        cont.Email AS RestaurantEmail,
                                        cont.PhoneNumber AS RestaurantPhoneNumber,
                                        tab.Capacity,
                                        cus.Name AS CustomerName,
                                        cus.LocationId AS CustomerLocationId,
                                        cus.ContactInformationId AS CustomerContactInfoId,
                                        cloc.PostalCode AS CustomerPostalCode,
                                        cloc.MunicipalityName AS CustomerMunicipality,
                                        cloc.StreetName AS CustomerStreet,
                                        cloc.HouseNumber AS CustomerHouseNumber,
                                        ccont.Email AS CustomerEmail,
                                        ccont.PhoneNumber AS CustomerPhoneNumber
                                    FROM Reservation r
                                    INNER JOIN Restaurant res ON r.RestaurantId = res.RestaurantId
                                    INNER JOIN Cuisine cu ON res.CuisineId = cu.CuisineId
                                    INNER JOIN Location loc ON res.LocationId = loc.LocationId
                                    INNER JOIN ContactInformation cont ON res.ContactInformationId = cont.ContactInformationId
                                    INNER JOIN [Table] tab ON r.TableNumber = tab.TableNumber
                                    INNER JOIN Customer cus ON r.CustomerNumber = cus.CustomerNumber
                                    INNER JOIN Location cloc ON cus.LocationId = cloc.LocationId
                                    INNER JOIN ContactInformation ccont ON cus.ContactInformationId = ccont.ContactInformationId
                                    WHERE r.RestaurantId = @RestaurantId
                                          AND r.ReservationDate = @Date";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        command.Parameters.AddWithValue("@Date", date);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Reservation reservation = new Reservation
                                {
                                    ReservationNumber = reader.GetInt32(reader.GetOrdinal("ReservationNumber")),
                                    StartHour = reader.GetTimeSpan(reader.GetOrdinal("StartHour")),
                                    EndHour = reader.GetTimeSpan(reader.GetOrdinal("EndHour")),
                                    AmountOfSeats = reader.GetInt32(reader.GetOrdinal("AmountOfSeats")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                    CustomerNumber = reader.GetInt32(reader.GetOrdinal("CustomerNumber")),
                                    RestaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantId")),
                                    TableNumber = reader.GetInt32(reader.GetOrdinal("TableNumber"))
                                };

                                reservation.Customer = new Customer
                                {
                                    CustomerNumber = reservation.CustomerNumber,
                                    Name = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    Location = new Location
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerLocationId")),
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("CustomerPostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("CustomerMunicipality")),
                                        StreetName = reader.IsDBNull(reader.GetOrdinal("CustomerStreet")) ? null : reader.GetString(reader.GetOrdinal("CustomerStreet")),
                                        HouseNumber = reader.IsDBNull(reader.GetOrdinal("CustomerHouseNumber")) ? null : reader.GetString(reader.GetOrdinal("CustomerHouseNumber")),
                                    },
                                    ContactInformation = new ContactInformation
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerContactInfoId")),
                                        Email = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("CustomerPhoneNumber")),
                                    }
                                };

                                reservation.Restaurant = new Restaurant
                                {
                                    RestaurantId = reservation.RestaurantId,
                                    Name = reader.GetString(reader.GetOrdinal("RestaurantName")),
                                    Cuisine = new Cuisine
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CuisineId")),
                                        CuisineType = reader.GetString(reader.GetOrdinal("CuisineType")),
                                    },
                                    Location = new Location
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("RestaurantLocationId")),
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("RestaurantPostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("RestaurantMunicipality")),
                                        StreetName = reader.IsDBNull(reader.GetOrdinal("RestaurantStreet")) ? null : reader.GetString(reader.GetOrdinal("RestaurantStreet")),
                                        HouseNumber = reader.IsDBNull(reader.GetOrdinal("RestaurantHouseNumber")) ? null : reader.GetString(reader.GetOrdinal("RestaurantHouseNumber")),
                                    },
                                    ContactInformation = new ContactInformation
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("RestaurantContactInfoId")),
                                        Email = reader.GetString(reader.GetOrdinal("RestaurantEmail")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("RestaurantPhoneNumber")),
                                    }
                                };

                                reservation.Table = new Table
                                {
                                    TableNumber = reservation.TableNumber,
                                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                };

                                reservationList.Add(reservation);
                            }
                        }
                    }
                }

                return reservationList;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error in GetReservationsAsync: {ex.Message}");
            }

            return null;
        }

        public async Task<List<Reservation>> GetRestaurantReservationsForPeriodAsync(int restaurantId, DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Reservation> reservationList = new List<Reservation>();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                                    SELECT 
                                        r.ReservationNumber,
                                        r.StartHour,
                                        r.EndHour,
                                        r.AmountOfSeats,
                                        r.ReservationDate,
                                        r.CustomerNumber,
                                        r.RestaurantId,
                                        r.TableNumber,
                                        res.Name AS RestaurantName,
                                        res.CuisineId,
                                        res.LocationId AS RestaurantLocationId,
                                        res.ContactInformationId AS RestaurantContactInfoId,
                                        cu.CuisineType,
                                        loc.PostalCode AS RestaurantPostalCode,
                                        loc.MunicipalityName AS RestaurantMunicipality,
                                        loc.StreetName AS RestaurantStreet,
                                        loc.HouseNumber AS RestaurantHouseNumber,
                                        cont.Email AS RestaurantEmail,
                                        cont.PhoneNumber AS RestaurantPhoneNumber,
                                        tab.Capacity,
                                        cus.Name AS CustomerName,
                                        cus.LocationId AS CustomerLocationId,
                                        cus.ContactInformationId AS CustomerContactInfoId,
                                        cloc.PostalCode AS CustomerPostalCode,
                                        cloc.MunicipalityName AS CustomerMunicipality,
                                        cloc.StreetName AS CustomerStreet,
                                        cloc.HouseNumber AS CustomerHouseNumber,
                                        ccont.Email AS CustomerEmail,
                                        ccont.PhoneNumber AS CustomerPhoneNumber
                                    FROM Reservation r
                                    INNER JOIN Restaurant res ON r.RestaurantId = res.RestaurantId
                                    INNER JOIN Cuisine cu ON res.CuisineId = cu.CuisineId
                                    INNER JOIN Location loc ON res.LocationId = loc.LocationId
                                    INNER JOIN ContactInformation cont ON res.ContactInformationId = cont.ContactInformationId
                                    INNER JOIN [Table] tab ON r.TableNumber = tab.TableNumber
                                    INNER JOIN Customer cus ON r.CustomerNumber = cus.CustomerNumber
                                    INNER JOIN Location cloc ON cus.LocationId = cloc.LocationId
                                    INNER JOIN ContactInformation ccont ON cus.ContactInformationId = ccont.ContactInformationId
                                    WHERE r.RestaurantId = @RestaurantId
                                          AND r.ReservationDate BETWEEN @StartDate AND @EndDate";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        command.Parameters.AddWithValue("StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Reservation reservation = new Reservation
                                {
                                    ReservationNumber = reader.GetInt32(reader.GetOrdinal("ReservationNumber")),
                                    StartHour = reader.GetTimeSpan(reader.GetOrdinal("StartHour")),
                                    EndHour = reader.GetTimeSpan(reader.GetOrdinal("EndHour")),
                                    AmountOfSeats = reader.GetInt32(reader.GetOrdinal("AmountOfSeats")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("ReservationDate")),
                                    CustomerNumber = reader.GetInt32(reader.GetOrdinal("CustomerNumber")),
                                    RestaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantId")),
                                    TableNumber = reader.GetInt32(reader.GetOrdinal("TableNumber"))
                                };

                                reservation.Customer = new Customer
                                {
                                    CustomerNumber = reservation.CustomerNumber,
                                    Name = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    Location = new Location
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerLocationId")),
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("CustomerPostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("CustomerMunicipality")),
                                        StreetName = reader.IsDBNull(reader.GetOrdinal("CustomerStreet")) ? null : reader.GetString(reader.GetOrdinal("CustomerStreet")),
                                        HouseNumber = reader.IsDBNull(reader.GetOrdinal("CustomerHouseNumber")) ? null : reader.GetString(reader.GetOrdinal("CustomerHouseNumber")),
                                    },
                                    ContactInformation = new ContactInformation
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerContactInfoId")),
                                        Email = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("CustomerPhoneNumber")),
                                    }
                                };

                                reservation.Restaurant = new Restaurant
                                {
                                    RestaurantId = reservation.RestaurantId,
                                    Name = reader.GetString(reader.GetOrdinal("RestaurantName")),
                                    Cuisine = new Cuisine
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CuisineId")),
                                        CuisineType = reader.GetString(reader.GetOrdinal("CuisineType")),
                                    },
                                    Location = new Location
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("RestaurantLocationId")),
                                        PostalCode = reader.GetInt32(reader.GetOrdinal("RestaurantPostalCode")),
                                        MunicipalityName = reader.GetString(reader.GetOrdinal("RestaurantMunicipality")),
                                        StreetName = reader.IsDBNull(reader.GetOrdinal("RestaurantStreet")) ? null : reader.GetString(reader.GetOrdinal("RestaurantStreet")),
                                        HouseNumber = reader.IsDBNull(reader.GetOrdinal("RestaurantHouseNumber")) ? null : reader.GetString(reader.GetOrdinal("RestaurantHouseNumber")),
                                    },
                                    ContactInformation = new ContactInformation
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("RestaurantContactInfoId")),
                                        Email = reader.GetString(reader.GetOrdinal("RestaurantEmail")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("RestaurantPhoneNumber")),
                                    }
                                };

                                reservation.Table = new Table
                                {
                                    TableNumber = reservation.TableNumber,
                                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                };

                                reservationList.Add(reservation);
                            }
                        }
                    }
                }

                return reservationList;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error in GetReservationsAsync: {ex.Message}");
            }

            return null;
        }
    }
}
