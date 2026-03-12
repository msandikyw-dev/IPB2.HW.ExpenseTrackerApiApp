using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblStudent
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string FatherName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string ContactNumber { get; set; } = null!;

    public bool IsDelete { get; set; }
}
