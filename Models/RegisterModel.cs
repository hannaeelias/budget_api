namespace budget_api.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public double? Salary { get; set; }
        public double? Balance { get; set; }
        public double? SavingsBalance { get; set; }
        public string UserName { get; set; }
    }

}
