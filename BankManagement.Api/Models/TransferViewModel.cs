using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Api.Models
{
    public class TransferViewModel
    {
        public long FromAccount { get; set; }

        public long ToAccount { get; set; }

        public double Amount { get; set; }
    }
}
