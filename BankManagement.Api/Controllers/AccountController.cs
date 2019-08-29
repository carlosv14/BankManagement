using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankManagement.Api.Models;
using BankManagement.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankManagement.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly BankManagementContext _bankManagementContext;
        public AccountController(BankManagementContext bankManagementContext)
        {
            this._bankManagementContext = bankManagementContext;
        }


        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public IEnumerable<AccountViewModel> Get()
        {
            return this._bankManagementContext.Accounts.Select(x => new AccountViewModel
            {
                Id = x.Id,
                Persona = $"{x.Person.FirstName} {x.Person.LastName}"
            });
        }
        
    }
}
