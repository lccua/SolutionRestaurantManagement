using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Serilog;

namespace RestaurantManagement.INITIALIZER
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;

            string logDirectory = Path.Combine("C:\\Users\\lucac\\source\\repos\\SolutionRestaurantManagement\\RestaurantManagement.INITIALIZER\\", "Log");
            string logPath = Path.Combine(logDirectory, "log.txt");

            // Create the log directory if it doesn't exist
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Create the log file if it doesn't exist
            if (!File.Exists(logPath))
            {
                File.Create(logPath).Close();
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath)
                .CreateLogger();
        }

        public void InitializeDatabase()
        {
            Log.Information("--------------------------------------------------");
            Log.Information($"Database initialization started at {DateTime.Now:yyyy-MM-dd HH:mm:ss}.");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    ExecuteScript(connection, "DropTable.sql");

                    ExecuteScript(connection, "CreateTable.sql");

                    ExecuteScript(connection, "InsertSeedData.sql");
                }

                Log.Information("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during database initialization.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void ExecuteScript(SqlConnection connection, string scriptFileName)
        {
            string scriptPath = Path.Combine("C:\\Users\\lucac\\source\\repos\\SolutionRestaurantManagement\\RestaurantManagement.INITIALIZER\\", "Script", scriptFileName);

            if (File.Exists(scriptPath))
            {
                string script = File.ReadAllText(scriptPath);

                using (SqlCommand command = new SqlCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }

                Log.Information($"Script '{scriptFileName}' executed successfully.");
            }
            else
            {
                Log.Error($"Script file '{scriptFileName}' not found.");
            }
        }
    }
}
