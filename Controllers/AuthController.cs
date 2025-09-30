using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResumeGenerator.Data;
using ResumeGenerator.Data.Models.Exceptions;
using ResumeGenerator.Data.Interfaces;

namespace ResumeGenerator.Controllers
{
    [Route("api/main1")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSender _emailSender;
        private readonly IJWTTokenCreator _jwtTokenCreator;

        public AuthController(UserManager<ApplicationUser> userManager, EmailSender emailSender, IJWTTokenCreator jwtTokenCreator)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _jwtTokenCreator = jwtTokenCreator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid) throw new ModelStateException("Please fill everything correctly");

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) throw new RegisterException("Register failed, please try again later");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Auth",
                new { userId = user.Id, token = encodedToken },
                Request.Scheme
            );

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.");

            return Ok(new { message = "User registered successfully" });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException("User not found");

            token = WebUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) throw new EmailException("Something went wrong, please try again");

            return Ok("Email confirmed successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid) throw new ModelStateException("Something went wrong.");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new UnauthorizedException("User not found");
            if (!await _userManager.IsEmailConfirmedAsync(user)) throw new EmailException("Please confirm the email!");
            if (!await _userManager.CheckPasswordAsync(user, model.Password)) throw new UnauthorizedException("Login didnt succeed");

            var token = _jwtTokenCreator.CreateToken(model.Email);
            return Ok(token);
        }
    }
}
//TODO 6 стилей и доделать инвойс, добавить стиоли в ди контейнер