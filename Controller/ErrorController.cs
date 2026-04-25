using Microsoft.AspNetCore.Mvc;

namespace UniversityApi.Controllers;

[ApiController]
[Route("/error")]
// Add swagger ignore attribute to prevent this endpoint from showing in the API documentation
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("")]
    public IActionResult HandleError()
    {
        // Return a generic error response
        return Problem(
            detail: "An unexpected error occurred. Please try again later.",
            title:"Internal Server Error",
            statusCode: 500
        );
    }
}