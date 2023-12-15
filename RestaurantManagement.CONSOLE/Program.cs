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
                        await PostCustomer(); // Assuming Post is also an async method
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

            int restaurantId;
            string date;
            Console.WriteLine("GET Restaurant Reservations by ID and DAY");
            Console.WriteLine("------------------------------------------");

            restaurantId = 1;

            date = "2024-11-25";

            Console.WriteLine("------------------------------------------");



            List<ReservationOutputUI> reservationsOutput = await adminService.GetRestaurantReservationsByDayAsync(restaurantId, date);

            foreach (var restaurantReservation in reservationsOutput)
            {
                Console.WriteLine(restaurantReservation.ToString());
            }

        }

        private async static Task PostCustomer()
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

            name = "Luca Cassier";

            email = "luca@gmail.com";

            phoneNumber = "0498531583";

            postalCode = 9940;

            municipality = "Evergem";

            streetName = "Garenstraat";

            houseNumber = "4a";

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

            Console.WriteLine();
            Console.WriteLine("LOCATION: " + uri);

        }
    }
}
