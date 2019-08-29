using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BankManagement.Database
{
    public abstract class Account
    {
        [Key]
        public long Id { get; set; }

        public double Balance { get; set; }

        public Currency Currency { get; set; }

        [ForeignKey(nameof(Person))]
        public long PersonId { get; set; }

        public virtual Person Person { get; set; }

    }
}