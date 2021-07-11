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
        public UserAccountController(UserAccountService accountService)
        {
            _accountService = accountService;
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
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
