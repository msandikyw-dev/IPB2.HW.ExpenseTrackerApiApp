using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblBlogDetail
{
    public int? BlogDetailId { get; set; }

    public int? BlogId { get; set; }

    public string? BlogContent { get; set; }
}
