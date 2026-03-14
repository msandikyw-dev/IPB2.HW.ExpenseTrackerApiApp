namespace IPB2.HW.ExpenseTrackerApiApp.Models.Budget
{
    public class BudgetRemainingResponseDTO
    {
        public int BudgetYear { get; set; }
        public int BudgetMonth { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal RemainingBudget { get; set; }
    }
}
