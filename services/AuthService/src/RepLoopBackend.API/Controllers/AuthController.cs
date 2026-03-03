using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepLoopBackend.Application.Features.Auth.Commands.AppleAuth;
using RepLoopBackend.Application.Features.Auth.Commands.ChangePassword;
using RepLoopBackend.Application.Features.Auth.Commands.ForgotPassword;
using RepLoopBackend.Application.Features.Auth.Commands.GoogleAuth;
using RepLoopBackend.Application.Features.Auth.Commands.Login;
using RepLoopBackend.Application.Features.Auth.Commands.Logout;
using RepLoopBackend.Application.Features.Auth.Commands.Register;
using RepLoopBackend.Application.Features.Auth.Commands.ResetPassword;

namespace RepLoopBackend.API.Controllers;

[Route("api/auth")]
public class AuthController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Eğer bu email ile bir hesap varsa, şifre sıfırlama bağlantısı gönderildi." });
    }

    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody] GoogleAuthCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("apple")]
    public async Task<IActionResult> Apple([FromBody] AppleAuthCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        await _mediator.Send(command with { UserId = CurrentUserId });
        return Ok(new { message = "Şifreniz başarıyla değiştirildi." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Şifreniz başarıyla sıfırlandı." });
    }
}
