namespace IPB2.HW.ExpenseTrackerApiApp.Models.Budget
{
    public class BudgetResponseDTO
    {
        public int BudgetId { get; set; }
        public int BudgetYear { get; set; }
        public int BudgetMonth { get; set; }
        public decimal BudgetAmount { get; set; }
    }
}
