namespace Restaurant.RestApi;

#pragma warning disable CA1515
public interface IReservationsRepository
#pragma warning restore CA1515
{
    Task Create(Reservation reservation);
}