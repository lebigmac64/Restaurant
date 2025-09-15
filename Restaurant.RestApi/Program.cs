using Restaurant.RestApi;
using Restaurant.RestApi.Persistence.Repositories;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            var services = builder.Services;
            var configuration = builder.Configuration;
            var connectionString = configuration.GetConnectionString("Restaurant");
            ArgumentException.ThrowIfNullOrEmpty(connectionString);
    
            services.AddControllers();
    
            MigrationTool.Migrate(connectionString);
    
            services.AddSingleton<IReservationsRepository>(
                new SqlReservationRepository(connectionString));
        }

        var app = builder.Build();
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
    
            app.UseRouting();
            app.MapControllers();
        }
        app.Run();
    }
}