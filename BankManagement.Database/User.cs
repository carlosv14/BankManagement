using System.ComponentModel.DataAnnotations;

namespace BankManagement.Database
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

    }
}