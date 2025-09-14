using Restaurant.RestApi;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    services.AddControllers();
    services.AddSingleton<IReservationsRepository, NullRepository>();
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

// A repository that does nothing, for vertical slice development.
// Later, we will replace it with a real database implementation.
internal class NullRepository : IReservationsRepository
{
    public Task Create(Reservation reservation)
    {
        return Task.CompletedTask;
    }
}