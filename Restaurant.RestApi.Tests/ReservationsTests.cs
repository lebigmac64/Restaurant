using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Restaurant.RestApi.Tests;

public class ReservationsTests
{
    [Fact]
    public async Task PostValidReservation()
    {
        var response = await PostReservation(new
        {
            at = "2023-03-10 19:00",
            email = "katinka@example.com",
            name = "Katinka Ingabogovinanana",
            quantity = 2
        });
        
        Assert.True(response.IsSuccessStatusCode,
            $"Actual status code: {response.StatusCode}.");
    }
    
    [SuppressMessage(
        "Usage", "CA2234:Pass system uri objects instead of strings",
        Justification = "URL isn't passed as variable, but as string literal.")]
    private static async Task<HttpResponseMessage> PostReservation(object reservation)
    {
        using var factory = new RestaurantApiFactory();
        var client = factory.CreateClient();
        
        var json = JsonSerializer.Serialize(reservation);
        using var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return await client.PostAsync("reservations", content);
    }

    [Theory]
    [InlineData("2023-03-10 19:00", "juliad@example.com", "Julia Doma", 5)]
    [InlineData("2024-07-20 05:00", "peterp@example.com", "Peter Parker", 2)]
    public async Task PostValidReservationWhenDatabaseIsEmpty(
        string at,
        string email,
        string name,
        int quantity)
    {
        var db = new FakeDatabase();
        var sut = new ReservationsController(db);

        var dto = new ReservationDto
        {
            At = at,
            Email = email,
            Name = name,
            Quantity = quantity
        };
        await sut.Post(dto);
        
        var expected = new Reservation
        (
            DateTime.Parse(dto.At, CultureInfo.InvariantCulture),
            dto.Email,
            dto.Name,
            dto.Quantity
        );
        Assert.Contains(expected, db);
    }
    
    [Theory]
    [InlineData(null, "danielb@example.com", "Daniel Brühl", 3)]
    [InlineData("2023-03-10 19:00", null, "Daniel Brühl", 3)]
    [InlineData("2023-03-10 19:00", "danielb@example.com", null, 3)]
    [InlineData("2023-03-10 19:00", "terezah@git.com", "Terezah Git", 0)]
    [InlineData("2023-03-10 19:00", "terezah@git.com", "Terezah Git", -1)]
    public async Task PostInvalidReservation(
        string? at,
        string? email,
        string? name,
        int quantity)
    {
        var response = await PostReservation(new
        {
            at,
            email,
            name,
            quantity
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}