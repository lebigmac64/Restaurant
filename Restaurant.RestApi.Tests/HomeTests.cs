using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Restaurant.RestApi.Tests;

public class HomeTests
{
    [Fact]
    [SuppressMessage(
        "Usage", "CA2234:Pass system uri objects instead of strings",
        Justification = "URL isn't passed as variable, but as string literal.")]
    public async Task HomeIsOk()
    {
        using var factory = new RestaurantApiFactory();
        var client = factory.CreateClient();

        using var request = new HttpRequestMessage(HttpMethod.Get, "");
        request.Headers.Add("Accept", "application/json");
        var response = await client
            .SendAsync(request);

        Assert.True(response.IsSuccessStatusCode,
            $"Actual status code: {response.StatusCode}.");
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }
}
