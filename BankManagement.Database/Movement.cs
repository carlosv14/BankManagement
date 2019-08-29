using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagement.Database
{
    public class Movement
    {

        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey(nameof(FromAccount))]
        public long FromAccountId { get; set; }

        public virtual Account FromAccount { get; set; }

        [ForeignKey(nameof(ToAccount))]
        public long ToAccountId { get; set; }

        public virtual Account ToAccount { get; set; }

        public double Amount { get; set; }

    }
}