using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_api.Models
{
    [Table("AppUser")]
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public double? Salary { get; set; }
        public double? Balance { get; set; }
        public double? SavingsBalance { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? Role { get; set; }


        public new string? UserName { get; set; } 
    }
}
