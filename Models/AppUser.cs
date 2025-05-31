using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_api.Models
{
    [Table("AppUser")]
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public double Salary { get; set; } = 0;
        public double Balance { get; set; } = 0;
        public double? SavingsBalance { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? Role { get; set; }


    }
}
