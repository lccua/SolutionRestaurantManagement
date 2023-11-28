using RestaurantManagement.API.DTO;
using RestaurantManagement.API.DTO.Table;
using RestaurantManagement.DOMAIN.Model;

namespace RestaurantManagement.API.Mapper
{
    public class TableMapper
    {
        public static Table ToTableDTO(TableInputDTO tableDTO)
        {
            try
            {
                return new Table
                {
                    Capacity = tableDTO.Capacity,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public static TableOutputDTO FromTable(Table table)
        {
            try
            {
                return new TableOutputDTO
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
