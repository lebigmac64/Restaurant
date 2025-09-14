using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Restaurant.RestApi.Tests;

[SuppressMessage(
    "Naming",
    "CA1710:Identifiers should have correct suffix")]
#pragma warning disable CA1515
public class FakeDatabase :
#pragma warning restore CA1515
    Collection<Reservation>, IReservationsRepository
{
    public Task Create(Reservation reservation)
    {
        Add(reservation);
        return Task.CompletedTask;
    }
}