using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.RestApi;

[Route("[controller]")]
[SuppressMessage(
    "Design", "CA1515:Because an application's API isn't typically referenced from outside the assembly, types can be made internal", 
    Justification = "Controllers must be public for ASP.NET Core routing.")]
public class ReservationsController
{
    public ReservationsController(IReservationsRepository repository)
    {
        Repository = repository;
    }
    
    public IReservationsRepository Repository { get; }
    
    public async Task Post(ReservationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        await Repository
            .Create(
                new Reservation(
                    new DateTime(2023, 11, 24,19, 0 ,0),
                    "juliad@example.net",
                    "Julia Doma",
                    5));
    }
}