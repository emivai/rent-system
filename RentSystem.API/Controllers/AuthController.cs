using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITokenManager _tokenManager;
        private readonly IValidator<RegisterUserDTO> _validator;
        public AuthController(IUserService userService, ITokenManager tokenManager, IValidator<RegisterUserDTO> validator)
        {
            _userService = userService;
            _tokenManager = tokenManager;
            _validator = validator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginUserDTO loginUserDTO)
        {
            return Ok(await _userService.LoginAsync(loginUserDTO));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            var result = _validator.Validate(registerUserDTO);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }

            await _userService.CreateAsync(registerUserDTO);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("currentUser")]
        public async Task<IActionResult> CurrentUser()
        {
            var bearerToken = Request.Headers["authorization"].ToString().Replace("Bearer ", "");
            var token = _tokenManager.DecodeAccessTokenAsync(bearerToken);

            if (token == null || token.ValidFrom > DateTime.UtcNow || token.ValidTo < DateTime.UtcNow)
            {
                return Unauthorized();
            }

            var userId = GetUserId();

            return Ok(await _userService.GetAsync(userId));
        }
    }
}
