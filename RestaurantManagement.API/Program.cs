using RestaurantManagement.DOMAIN.Manager;
using RestaurantManagement.DATA.Repository;
using RestaurantManagement.DOMAIN.Interface;

namespace RestaurantManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = "Data Source=DESKTOP-C2MADIB;Initial Catalog=RestaurantDB;Integrated Security=True;";

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddSingleton<ICustomerRepository>(c => new CustomerRepository(connectionString));
            builder.Services.AddSingleton<CustomerManager>();

            builder.Services.AddSingleton<IReservationRepository>(r => new ReservationRepository(connectionString));
            builder.Services.AddSingleton<ReservationManager>();

            builder.Services.AddSingleton<IRestaurantRepository>(r => new RestaurantRepository(connectionString));
            builder.Services.AddSingleton<RestaurantManager>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
