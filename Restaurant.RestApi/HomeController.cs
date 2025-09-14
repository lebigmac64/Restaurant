using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.RestApi;

[Route("")]
[SuppressMessage(
    "Design", "CA1515:Because an application's API isn't typically referenced from outside the assembly, types can be made internal", 
    Justification = "Controllers must be public for ASP.NET Core routing.")]
public class HomeController : ControllerBase
{
    public IActionResult Get()
    {
        return Ok(new {message = "Hello World!"});
    }
}