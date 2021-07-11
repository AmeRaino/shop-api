using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos.Accounts;
using ShopApi.Dtos.Users;
using ShopApi.Extensions;
using ShopApi.Models.Users;
using ShopApi.Services;
using ShopApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserAccountService _accountService;
        private readonly UserService _userService;
        public UserController(UserAccountService accountService, UserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }


        // Authenticate with registered account
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(AccountAuthenticateModel model)
        {
            try
            {
                var user = await _accountService.AuthenticatAccount(model.Username, model.Password);
                var response = new UserResponseModel();
                response.CopyPropertiesFrom(user);
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(AccountRegisterModel model)
        {
            try
            {
                var user = new User();
                user.CopyPropertiesFrom(model);
                user = _userService.Create(user);

                var account = new UserAccount();
                account.CopyPropertiesFrom(model);
                account.User = user;
                account = await _accountService.Register(account, model.Password);

                var response = new UserResponseModel();
                response.CopyPropertiesFrom(user);

                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
