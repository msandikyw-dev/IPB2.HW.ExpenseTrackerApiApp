using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblAccount
{
    public string AccountId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public decimal Balance { get; set; }

    public bool IsDelete { get; set; }

    public string Password { get; set; } = null!;
}
