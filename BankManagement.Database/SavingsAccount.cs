using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement.Database
{
    public class SavingsAccount : Account
    {
        public float InterestRate { get; private set; } = 5f;
    }
}
