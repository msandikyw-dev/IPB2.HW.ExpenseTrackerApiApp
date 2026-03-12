using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblExpense
{
    public int ExpenseId { get; set; }

    public DateTime ExpenseDate { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public bool IsDelete { get; set; }

    public virtual TblCategory Category { get; set; } = null!;
}
