using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblTransferRecord
{
    public string TranId { get; set; } = null!;

    public string FromMobile { get; set; } = null!;

    public string ToMobile { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime TimeStamp { get; set; }

    public bool IsDelete { get; set; }
}
