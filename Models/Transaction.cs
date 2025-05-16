using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_api.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Transaction")]
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserId { get; set; }  
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }  

        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public Item? Item { get; set; }

        public double Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Type { get; set; } = "Expense";
        public bool IsDeleted { get; set; } = false;
    }

}
