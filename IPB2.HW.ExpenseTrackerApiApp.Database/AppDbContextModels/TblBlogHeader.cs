using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblBlogHeader
{
    public int? BlogId { get; set; }

    public string? BlogTitle { get; set; }
}
