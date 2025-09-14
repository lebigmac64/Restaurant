using System.Diagnostics.CodeAnalysis;
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
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        
        string json = JsonSerializer.Serialize(reservation);
        using var content = new StringContent(json);
        content.Headers.ContentType = new("application/json");
        return await client.PostAsync("reservations", content);
    }
}