using RestaurantManagement.API.DTO;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public class TableMapper
    {
        public static Table ToTableDTO(TableDTO tableDTO)
        {
            try
            {
                return new Table
                {
                    TableNumber = tableDTO.TableNumber,
                    Capacity = tableDTO.Capacity,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static TableDTO FromTable(Table table)
        {
            try
            {
                return new TableDTO
                {
                    TableNumber = table.TableNumber,
                    Capacity = table.Capacity,


                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
