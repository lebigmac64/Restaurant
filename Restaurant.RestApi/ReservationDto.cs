namespace Restaurant.RestApi;

#pragma warning disable CA1515
public class ReservationDto
#pragma warning restore CA1515
{
    public string? At { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
}