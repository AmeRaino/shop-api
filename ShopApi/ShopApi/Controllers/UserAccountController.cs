using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos.Accounts;
using ShopApi.Extensions;
using ShopApi.Models.Users;
using ShopApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly UserAccountService _accountService;
        private readonly UserService _userService;
        public UserAccountController(UserAccountService accountService, UserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }


        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(AccountAuthenticateModel model)
        {
            try
            {
                var user = await _accountService.AuthenticateUserAccount(model.Username, model.Password);
                return Ok(user);
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
                user = await _userService.Create(user);

                var account = new UserAccount();
                account.CopyPropertiesFrom(model);
                account.User = user;
                account = await _accountService.Register(account, model.Password);
                return Ok(account);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
