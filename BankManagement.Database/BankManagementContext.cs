using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Database
{
    public class BankManagementContext : DbContext
    {
        public BankManagementContext(DbContextOptions options) 
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<SavingsAccount> SavingsAccounts { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<Movement> Movements { get; set; }

        public virtual DbSet<User> Users { get; set; }
    }
}
