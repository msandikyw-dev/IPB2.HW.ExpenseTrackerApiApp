using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblBudget
{
    public int BudgetId { get; set; }

    public int BudgetYear { get; set; }

    public int BudgetMonth { get; set; }

    public decimal BudgetAmount { get; set; }

    public bool IsDelete { get; set; }
}
