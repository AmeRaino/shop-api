using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos.Authentications;
using ShopApi.Dtos.Registers;
using ShopApi.Dtos.Users;
using ShopApi.Extensions;
using ShopApi.Services.Users;
using System;
using System.Threading.Tasks;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        //private readonly AuthProviderService _authProviderService;

        public UsersController(
            IUserService userService)
        {
            _userService = userService;
        }


        // Authenticate with registered account
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            try
            {
                var response = await _userService.AuthenticateAsync(model);
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Authenticate with third-party identity provider
        [HttpPost("third-party-authenticate")]
        public async Task<IActionResult> Authenticate(ThirdPartyAuthenticateRequest model)
        {
            try
            {
                var response = await _userService.Authenticate3rdPartyAsync(model);
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest model)
        {
            try
            {
                var user = await _userService.RegisterAsync(model);
                var response = new UserResponse();
                response.CopyPropertiesFrom(user);
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            var response = new UserResponse();
            response.CopyPropertiesFrom(user);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]UpdateUserModel model)
        {
            try
            {
                _userService.Update(id, model);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
