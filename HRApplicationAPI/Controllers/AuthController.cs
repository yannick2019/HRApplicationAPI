using AutoMapper;
using HRApplicationAPI.Interfaces;
using HRApplicationAPI.Models.DbModels;
using HRApplicationAPI.Models.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRApplicationAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IMapper mapper)
        {
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult<string> Register(RegisterInputModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userModel.Password != userModel.ConfirmedPassword)
                    {
                        return BadRequest("Passwords does not match!");
                    }

                    if (_authService.DoesUserExists(userModel.Email))
                    {
                        return BadRequest("User already exists!");
                    }

                    var mappedModel = _mapper.Map<RegisterInputModel, User>(userModel);
                    mappedModel.Role = "User";

                    var user = _authService.RegisterUser(mappedModel);
                    if (user != null)
                    {
                        var token = _authService.GenerateJwtToken(user.Email, mappedModel.Role);
                        return Ok(token);
                    }

                    return BadRequest("Email or password are not correct!");
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<string> Login(LoginInputModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_authService.IsAuthenticated(userModel.Email, userModel.Password))
                    {
                        var user = _authService.GetByEmail(userModel.Email);
                        var token = _authService.GenerateJwtToken(userModel.Email, user!.Role);

                        return Ok(token);
                    }

                    return BadRequest("Email or password are not correct!");
                }

                return BadRequest(ModelState);

            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
