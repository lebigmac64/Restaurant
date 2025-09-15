using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Restaurant.RestApi.Tests;

public class ReservationsTests
{
    [Fact]
    public async Task PostValidReservation()
    {
        var response = await PostReservation(new
        {
            date = "2023-03-10 19:00",
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
        
        string json = JsonSerializer.Serialize(reservation);
        using var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return await client.PostAsync("reservations", content);
    }

    [Fact]
    public async Task PostValidReservationWhenDatabaseIsEmpty()
    {
        var db = new FakeDatabase();
        var sut = new ReservationsController(db);

        var dto = new ReservationDto
        {
            At = "2023-11-24 19:00",
            Email = "juliad@example.net",
            Name = "Julia Doma",
            Quantity = 5
        };
        await sut.Post(dto);
        
        var expected = new Reservation
        (
            new DateTime(2023, 11, 24,19, 0 ,0),
            dto.Email,
            dto.Name,
            dto.Quantity
        );
        Assert.Contains(expected, db);
    }
}