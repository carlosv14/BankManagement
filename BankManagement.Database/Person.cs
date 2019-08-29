using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagement.Database
{
    public class Person
    {

        [Key]
        public long Id { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}