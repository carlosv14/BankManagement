using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagement.Api.Models
{
    public class HomePageUserInformation
    {
        public string Name { get; set; }

        public double Balance { get; set; }

        public IEnumerable<TransferViewModel> Movements { get; set; }
    }
}
