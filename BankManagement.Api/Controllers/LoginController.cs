using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BankManagement.Api.Models;
using BankManagement.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankManagement.Api.Controllers
{
    [Route("api/[controller]")]

    public class LoginController : Controller
    {
        private IConfiguration _config;
        private readonly BankManagementContext _bankManagementContext;

        public LoginController(IConfiguration config, BankManagementContext bankManagementContext)
        {
            _config = config;
            this._bankManagementContext = bankManagementContext;
            if (!this._bankManagementContext.Users.Any())
            {
                this._bankManagementContext.Users.Add(new User
                {
                    Username = "admin",
                    Password = "admin"
                });
            }

            this._bankManagementContext.SaveChanges();
        }


        /// <summary>
        /// Autentica al usuario y retorna el token que lo identifica.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserLoginViewModel login)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(login);

            if (user == null) return response;
            var tokenString = GenerateJsonWebToken(user);
            response = Ok(new { token = tokenString, isAdmin = user.Username == "admin" });

            return response;
        }

        private string GenerateJsonWebToken(UserInformationViewModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token;

            if (userInfo.Username == "admin")
            {
                token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    new Claim[]
                    {
                        new Claim("Administrator", ""),
                        new Claim("UserName", userInfo.Username)
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);
            }
            else
            {
                token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    null,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);
            }

            

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserInformationViewModel> AuthenticateUser(UserLoginViewModel login)
        {
            var user = await this._bankManagementContext.Users.FirstOrDefaultAsync(x => x.Username == login.Username && x.Password == login.Password);
            var person = await this._bankManagementContext.Persons.FirstOrDefaultAsync(x => x.UserId == user.Id);

            return login.Username != null ? new UserInformationViewModel { Username = user.Username, Name  = $"{person?.FirstName ?? ""} {person?.LastName ?? ""}"} : null;
        }
    }
}

