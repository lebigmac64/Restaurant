using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.RestApi;

[Route("[controller]")]
[SuppressMessage(
    "Design", "CA1515:Because an application's API isn't typically referenced from outside the assembly, types can be made internal", 
    Justification = "Controllers must be public for ASP.NET Core routing.")]
public class ReservationsController
{
    private readonly IReservationsRepository _reservationsRepository;
    
    public ReservationsController(IReservationsRepository repository)
    {
        _reservationsRepository = repository;
    }
    
    public async Task Post([FromBody] ReservationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var r = new Reservation(
            DateTime.Parse(dto.At!, CultureInfo.InvariantCulture),
            dto.Email!,
            dto.Name!,
            dto.Quantity);
        
        await _reservationsRepository.Create(r);
    }
}