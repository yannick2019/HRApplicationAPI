using AutoMapper;
using HRApplicationAPI.Models.DbModels;
using HRApplicationAPI.Models.InputModels;
using HRApplicationAPI.Models.OutputModels;
using HRApplicationAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRApplicationAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly AuthService _authService;
        private readonly IMapper _mapper;

        public UsersController(ILogger logger, AuthService authService, IMapper mapper)
        {
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<UserOutputModel[]> GetAll()
        {
            try
            {
                var users = _authService.GetAll();
                var mappedUsers = _mapper.Map<User[], UserOutputModel[]>(users);
                return Ok(mappedUsers);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPost("role")]
        public ActionResult<UserOutputModel> ChangeRole(UserChangeRoleInputModel model)
        {
            try
            {
                var user = _authService.ChangeRole(model.Email, model.Role);
                var mappedUser = _mapper.Map<User, UserOutputModel>(user!);
                return Ok(mappedUser);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
