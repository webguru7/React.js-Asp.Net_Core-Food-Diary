using System.Threading;
using System.Threading.Tasks;
using FoodDiary.Application.Features.Auth.SignInWithGoogle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.API.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet("google")]
    public async Task<IActionResult> Google([FromBody] SignInWithGoogleRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(request, cancellationToken);
        return Ok();
    }
}