using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblBurmeseRecipe
{
    public string Guid { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Ingredients { get; set; }

    public string? CookingInstructions { get; set; }

    public int UserTypeId { get; set; }

    public bool IsDelete { get; set; }

    public virtual TblUserType UserType { get; set; } = null!;
}
