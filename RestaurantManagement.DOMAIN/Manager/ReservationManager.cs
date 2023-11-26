using RestaurantManagement.DOMAIN.Interface;
using RestaurantManagement.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DOMAIN.Manager
{
    public class ReservationManager
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationManager(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            List<Table> availableTables = await _reservationRepository.GetAvailableTables(reservation.Date, reservation.Hour, reservation.Restaurant.RestaurantId);

            Table SelectedTable = SelectTable(availableTables, reservation.AmountOfSeats);


            await _reservationRepository.AddReservationAsync(reservation);
        }

        public Table SelectTable (List<Table> availableTables, int amountOfSeats)
        {
            Table selectedTable = new Table();
            try
            {            
                if (availableTables.Count > 0)
                {
                    // Sort the available tables by the absolute difference between their capacity and the reservation's amount of seats
                    availableTables.Sort((table1, table2) =>
                    {
                        int diff1 = Math.Abs(table1.Capacity - amountOfSeats);
                        int diff2 = Math.Abs(table2.Capacity - amountOfSeats);
                        return diff1.CompareTo(diff2);
                    });

                    // The first table in the sorted list is the one with the capacity closest to the amount of seats
                    selectedTable = availableTables[0];
                    
                }
                else { throw new SystemException("No available tables for the given date, hour, and restaurant."); }

            }
            catch (SystemException ex) { Console.WriteLine($"Error: {ex.Message}"); }
            catch (System.Exception ex) { Console.WriteLine($"An error occurred: {ex.Message}"); }

            return selectedTable;
        }
    }
}
