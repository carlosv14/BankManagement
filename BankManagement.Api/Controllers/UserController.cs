using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BankManagement.Api.Models;
using BankManagement.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly BankManagementContext _bankManagementContext;

        public UserController(BankManagementContext bankManagementContext)
        {
            this._bankManagementContext = bankManagementContext;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<UserInformationViewModel> Get()
        {
            return this._bankManagementContext.Persons.Include(x => x.User).Select(x => new UserInformationViewModel
            {
                Username = x.User.Username,
                Name = $"{x.FirstName} {x.LastName}"
            });
        }


        [HttpGet]
        [Route("UserInfo")]
        public async Task<HomePageUserInformation> GetUserInfo()
        {
            if (!(User.Identity is ClaimsIdentity claimsIdentity)) throw new Exception("User doesn't exist");
            var userName = claimsIdentity.FindFirst("UserName").Value;
            var user = await this._bankManagementContext.Users.FirstOrDefaultAsync(x => x.Username == userName);
            var person = await this._bankManagementContext.Persons.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var movements = this._bankManagementContext.Movements.Where(x =>
                x.FromAccount.PersonId == person.Id || x.ToAccount.PersonId == person.Id);
            return new HomePageUserInformation
            {
                Name = $"{person.FirstName} {person.LastName}",
                Balance = person.Accounts.First().Balance,
                Movements = movements.Select(x => new TransferViewModel
                {
                    Amount = x.Amount,
                    ToAccount = x.ToAccountId,
                    FromAccount =  x.FromAccountId
                })
            };
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = "AdminUsersOnly")]
        public async Task<ActionResult> Post([FromBody]NewUserInformationViewModel user)
        {
            var existingUser =
                await this._bankManagementContext.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
            if (existingUser != null)
            {
                throw new Exception("Ese usuario ya existe");
            }

            var newUser = new User
            {
                Username = user.Username,
                Password = user.Password
            };

            var newPerson = new Person
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                User = newUser,
                Accounts = new List<Account>()
                {
                    new SavingsAccount
                    {
                        Currency = (Currency) user.Currency,
                        Balance = user.OpenningAmount,

                    }
                }
            };

            this._bankManagementContext.Persons.Add(newPerson);
            await this._bankManagementContext.SaveChangesAsync();
            return this.Ok();

        }
    }
}
