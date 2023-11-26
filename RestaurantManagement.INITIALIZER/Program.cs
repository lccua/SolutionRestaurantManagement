namespace RestaurantManagement.INITIALIZER
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=DESKTOP-C2MADIB;Initial Catalog=RestaurantDB;Integrated Security=True";
            DatabaseInitializer initializer = new DatabaseInitializer(connectionString);
            initializer.InitializeDatabase();

            Console.WriteLine("Initialization completed");
            Console.WriteLine("Log file can be found at: C:\\Users\\lucac\\source\\repos\\SolutionRestaurantManagement\\RestaurantManagement.INITIALIZER\\Log\\log.txt");
        }
    }
}
