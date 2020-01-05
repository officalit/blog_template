using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BLOG_CORE.Entity;
using BLOG_CORE.Helpers;
using BLOG_CORE.Model;
using BLOG_CORE.Model.Account;
using BLOG_CORE.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BLOG_CORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly Account accountModel = new Account();

        [Route("authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody]AccountAuthenticateVM userVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {  //кидаем исключения из модельки
                    return BadRequest("Модель не валидна");
                }
                //верификация юзера
                var user = accountModel.Authenticate(userVM.Username, userVM.Password);
                //возвращаем залогинившегося юзера с данными по аккаунту и токеном
                return Ok(new
                {
                    user.Id,
                    user.Username,
                    user.FirstName,
                    user.LastName,
                    user.Roles,
                    Token = accountModel.CreateToken(user)
                });
            }

            catch (AppException ex)
            {
                //кидаем исключения из модельки
                return BadRequest(ex.Message);
            }
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromForm]AccountRegisterVM userVM)
        {
            //мапим вьюмодель в сущность. далее подключу автомаппер
            User user = new User()
            {
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                Username = userVM.Username,
            };
            
            // создаём юзера
            try
            {
                return Ok(accountModel.AccountCreate(user, userVM.Password));
            }
            catch (AppException ex)
            {
                //кидаем исключения из модельки
                return BadRequest(ex.Message);
            }
        }
    }
}