using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    
    [SuppressMessage(
    "Usage", 
    "CA2208:Instantiate argument exceptions correctly", 
    Justification = "Quantity is a property of dto and is relevant for this exception.")]
    public async Task<ActionResult> Post([FromBody] ReservationDto dto)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(dto.At);
            ArgumentNullException.ThrowIfNull(dto.Email);
            ArgumentNullException.ThrowIfNull(dto.Name);
            if (dto.Quantity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.Quantity));
            }
        }
        catch (ArgumentNullException)
        {
            return new BadRequestResult();
        }

        var r = new Reservation(
            DateTime.Parse(dto.At, CultureInfo.InvariantCulture),
            dto.Email!,
            dto.Name!,
            dto.Quantity);
        
        await _reservationsRepository.Create(r);

        return new NoContentResult();
    }
}