namespace IPB2.HW.ExpenseTrackerApiApp.Models.Budget
{
    public class BudgetUpdateRequestDTO
    {
        public int BudgetYear { get; set; }
        public int BudgetMonth { get; set; }
        public decimal BudgetAmount { get; set; }
    }
}
