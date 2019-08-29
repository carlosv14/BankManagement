using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TransactionController : Controller
    {
        private readonly BankManagementContext _bankManagementContext;
        public TransactionController(BankManagementContext bankManagementContext)
        {
            this._bankManagementContext = bankManagementContext;
        }

        [HttpPost]
        public async Task<ActionResult> Transfer([FromBody] TransferViewModel transferInfo)
        {
            if (transferInfo == null)
            {
                throw new ArgumentNullException();
            }

            var fromAccount =
                await this._bankManagementContext.Accounts.FirstOrDefaultAsync(x => x.Id == transferInfo.FromAccount);

            var toAccount =
                await this._bankManagementContext.Accounts.FirstOrDefaultAsync(x => x.Id == transferInfo.ToAccount);

            if (fromAccount == null || toAccount == null)
            {
                throw new Exception("Las cuentas no existen");
            }

            if (fromAccount.Currency != toAccount.Currency)
            {
                throw new Exception("Las cuentas no tienen la misma moneda");
            }

            if (fromAccount.Balance < transferInfo.Amount)
            {
                throw new Exception("No hay suficiente dinero en la cuenta");
            }

            fromAccount.Balance -= transferInfo.Amount;
            this._bankManagementContext.Update(fromAccount);
            toAccount.Balance += transferInfo.Amount;
            this._bankManagementContext.Update(toAccount);
           this._bankManagementContext.Movements.Add(new Movement
            {
                ToAccountId = toAccount.Id,
                FromAccountId =  fromAccount.Id,
                Date = DateTime.Now,
                Amount = transferInfo.Amount
            });
            await this._bankManagementContext.SaveChangesAsync();
            return this.Ok();
        }

    }
}
