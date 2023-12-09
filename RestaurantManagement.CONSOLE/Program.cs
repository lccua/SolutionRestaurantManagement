using RestaurantManagement.CONSOLE.Model.Input;
using RestaurantManagement.CONSOLE.Model.Output;
using RestaurantManagement.CONSOLE.Service;
using System;

namespace RestaurantManagement.CONSOLE
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int input;
            do
            {
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Press 1 for GET | Press 2 for POST | Press 0 to EXIT");
                input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        await GetRestaurantReservationsByDayAsync();
                        break;

                    case 2:
                        await Post(); // Assuming Post is also an async method
                        break;

                    case 0:
                        Console.WriteLine("Exiting program...");
                        break;

                    default:
                        Console.WriteLine("Incorrect input. Please enter 1, 2, or 0.");
                        break;
                }

            } while (input != 0);
        }

        private async static Task GetRestaurantReservationsByDayAsync()
        {
            AdminServiceClient adminService = new AdminServiceClient();

            int id;
            string date;
            Console.WriteLine("GET Restaurant Reservations by ID and DAY");
            Console.WriteLine("------------------------------------------");

            Console.WriteLine("Enter Restaurant ID:");
            id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter DAY (ex. yyyy-mm-dd):");
            date = Console.ReadLine();

            Console.WriteLine("------------------------------------------");



            List<ReservationOutputUI> reservationsOutput = await adminService.GetRestaurantReservationsByDayAsync(id,date);

            foreach (var restaurantReservation in reservationsOutput)
            {
                Console.WriteLine(restaurantReservation.ToString());
            }

        }

        private async static Task Post()
        {
            CustomerServiceClient customerService = new CustomerServiceClient();

            string name;
            string email;
            string phoneNumber;
            int postalCode;
            string municipality;
            string streetName;
            string houseNumber;

            Console.WriteLine("POST Customer");
            Console.WriteLine("------------------------------------------");

            Console.WriteLine("Enter NAME:");
            name = Console.ReadLine();

            Console.WriteLine("Enter EMAIL:");
            email = Console.ReadLine();

            Console.WriteLine("Enter PHONENUMBER:");
            phoneNumber = Console.ReadLine();

            Console.WriteLine("Enter POSTALCODE:");
            postalCode = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter MUNICIPALITY:");
            municipality = Console.ReadLine();

            Console.WriteLine("Enter STREETNAME:");
            streetName = Console.ReadLine();

            Console.WriteLine("Enter HOUSENUMBER:");
            houseNumber = Console.ReadLine();

            LocationInputUI locationInput = new LocationInputUI();
            locationInput.PostalCode = postalCode;
            locationInput.MunicipalityName = municipality;
            locationInput.StreetName = streetName;
            locationInput.HouseNumber = houseNumber;

            ContactInformationInputUI contactInformationInput = new ContactInformationInputUI();
            contactInformationInput.Email = email;
            contactInformationInput.PhoneNumber = phoneNumber;

            CustomerInputUI customerInput = new CustomerInputUI();
            customerInput.Name = name;
            customerInput.ContactInformationInput = contactInformationInput;
            customerInput.LocationInput = locationInput;

            Uri uri = await customerService.AddCustomerAysnc(customerInput);

            Console.WriteLine(uri);


        }
    }
}
