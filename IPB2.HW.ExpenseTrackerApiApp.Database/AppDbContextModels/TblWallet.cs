using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblWallet
{
    public string Id { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public decimal Balance { get; set; }

    public bool IsDelete { get; set; }

    public string? Password { get; set; }
}
