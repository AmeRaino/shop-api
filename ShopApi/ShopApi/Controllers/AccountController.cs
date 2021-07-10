using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Dtos.Account;
using ShopApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(AccountAuthenticate model)
        {
            try
            {
                var user = await _accountService.AuthenticateUserAccount(model.Username, model.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(AccountRegister model)
        //{

        //}
    }
}
