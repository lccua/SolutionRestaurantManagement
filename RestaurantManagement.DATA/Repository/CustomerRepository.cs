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
    public class CustomerRepository : ICustomerRepository
    {
        private string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task RegisterCustomerAsync(Customer customer)
        {
            try
            {
                string insertContactInformationSQL = "INSERT INTO ContactInformation(Email, PhoneNumber) OUTPUT INSERTED.ContactInformationId VALUES(@email, @phoneNumber)";
                string insertLocationSQL = "INSERT INTO Location(PostalCode, MunicipalityName, StreetName, HouseNumber) OUTPUT INSERTED.LocationId VALUES(@postalCode, @municipalityName, @streetName, @houseNumber)";
                string insertCustomerSQL = "INSERT INTO Customer(Name, ContactId, LocationId) VALUES(@name, @contactId, @locationId)";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        // Insert ContactInformation
                        cmd.CommandText = insertContactInformationSQL;
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@email", customer.ContactInformation.Email);
                        cmd.Parameters.AddWithValue("@phoneNumber", customer.ContactInformation.PhoneNumber);
                        int contactId = (int)await cmd.ExecuteScalarAsync();

                        // Insert Location
                        cmd.CommandText = insertLocationSQL;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@postalCode", customer.Location.PostalCode);
                        cmd.Parameters.AddWithValue("@municipalityName", customer.Location.MunicipalityName);
                        cmd.Parameters.AddWithValue("@streetName", customer.Location.StreetName);
                        cmd.Parameters.AddWithValue("@houseNumber", customer.Location.HouseNumber);
                        int locationId = (int)await cmd.ExecuteScalarAsync();

                        // Insert Customer
                        cmd.CommandText = insertCustomerSQL;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@name", customer.Name);
                        cmd.Parameters.AddWithValue("@contactId", contactId);
                        cmd.Parameters.AddWithValue("@locationId", locationId);

                        await cmd.ExecuteNonQueryAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error registering customer.", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error registering customer", ex);
            }
        }

        public async Task DeleteCustomerAsync(int customerNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Customer SET IsActive = 0 WHERE CustomerNumber = @CustomerNumber";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerNumber", customerNumber);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Customer> GetCustomerAsync(int customerNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                                    SELECT 
                                        c.CustomerNumber,
                                        c.Name,
                                        ci.ContactInformationId,
                                        ci.Email,
                                        ci.PhoneNumber,
                                        l.LocationId,
                                        l.PostalCode,
                                        l.MunicipalityName,
                                        l.StreetName,
                                        l.HouseNumber
                                    FROM Customer c
                                    JOIN ContactInformation ci ON c.ContactId = ci.ContactInformationId
                                    JOIN Location l ON c.LocationId = l.LocationId
                                    WHERE c.CustomerNumber = @CustomerNumber
                                      AND c.IsActive = 1";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerNumber", customerNumber);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ContactInformation contactInformation = new ContactInformation((int)reader["ContactInformationId"], (string)reader["Email"], (string)reader["PhoneNumber"]);

                                Location location = new Location((int)reader["LocationId"], (int)reader["PostalCode"], (string)reader["MunicipalityName"], (string)reader["StreetName"], (string)reader["HouseNumber"]);

                                Customer customer = new Customer((int)reader["CustomerNumber"], (string)reader["Name"], contactInformation, location);

                                return customer; // Return the created Customer object
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, throw, etc.)
                // You may want to customize this based on your application's error handling strategy
                Console.WriteLine($"Error in GetCustomerAsync: {ex.Message}");
            }

            return null; // Customer not found or an error occurred
        }


        public async Task UpdateCustomerAsync(Customer updatedCustomer)
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
                                                        UPDATE ContactInformation 
                                                        SET 
                                                            Email = @Email,
                                                            PhoneNumber = @PhoneNumber
                                                        WHERE ContactInformationId = @ContactInformationId";

                            using (SqlCommand contactCommand = new SqlCommand(updateContactQuery, connection, transaction))
                            {
                                contactCommand.Parameters.AddWithValue("@Email", updatedCustomer.ContactInformation.Email);
                                contactCommand.Parameters.AddWithValue("@PhoneNumber", updatedCustomer.ContactInformation.PhoneNumber);
                                contactCommand.Parameters.AddWithValue("@ContactInformationId", updatedCustomer.ContactInformation.Id);


                                await contactCommand.ExecuteNonQueryAsync();
                            }

                            // Update Location
                            string updateLocationQuery = @"
                                                         UPDATE Location 
                                                         SET 
                                                             PostalCode = @PostalCode,
                                                             MunicipalityName = @MunicipalityName,
                                                             StreetName = @StreetName,
                                                             HouseNumber = @HouseNumber
                                                         WHERE LocationId = @LocationId";

                            using (SqlCommand locationCommand = new SqlCommand(updateLocationQuery, connection, transaction))
                            {
                                locationCommand.Parameters.AddWithValue("@PostalCode", updatedCustomer.Location.PostalCode);
                                locationCommand.Parameters.AddWithValue("@MunicipalityName", updatedCustomer.Location.MunicipalityName);
                                locationCommand.Parameters.AddWithValue("@StreetName", updatedCustomer.Location.StreetName);
                                locationCommand.Parameters.AddWithValue("@HouseNumber", updatedCustomer.Location.HouseNumber);
                                locationCommand.Parameters.AddWithValue("@LocationId", updatedCustomer.Location.Id);



                                await locationCommand.ExecuteNonQueryAsync();
                            }

                            // Update Customer
                            string updateCustomerQuery = @"
                                                         UPDATE Customer 
                                                         SET 
                                                             Name = @Name,
                                                             IsActive = @IsActive,
                                                             ContactId = @ContactId,
                                                             LocationId = @LocationId
                                                         WHERE CustomerNumber = @CustomerNumber";

                            using (SqlCommand customerCommand = new SqlCommand(updateCustomerQuery, connection, transaction))
                            {
                                customerCommand.Parameters.AddWithValue("@Name", updatedCustomer.Name);
                                customerCommand.Parameters.AddWithValue("@IsActive", updatedCustomer.IsActive);
                                customerCommand.Parameters.AddWithValue("@ContactId", updatedCustomer.ContactInformation.Id);
                                customerCommand.Parameters.AddWithValue("@LocationId", updatedCustomer.Location.Id);
                                customerCommand.Parameters.AddWithValue("@CustomerNumber", updatedCustomer.CustomerNumber);

                                await customerCommand.ExecuteNonQueryAsync();
                            }

                            // If everything is successful, commit the transaction
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // An error occurred, rollback the transaction
                            transaction.Rollback();

                            Console.WriteLine($"Error in UpdateCustomerAsync: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateCustomerAsync: {ex.Message}");
                throw;
            }
        }

    }
}
