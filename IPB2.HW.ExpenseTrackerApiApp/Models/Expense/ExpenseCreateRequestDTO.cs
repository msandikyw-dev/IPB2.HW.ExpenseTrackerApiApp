using System;

namespace IPB2.HW.ExpenseTrackerApiApp.Models.Expense
{
    public class ExpenseCreateRequestDTO
    {
        public DateTime ExpenseDate { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
    }
}
