using System;
using System.Collections.Generic;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class TblUserType
{
    public int UserId { get; set; }

    public string UserCode { get; set; } = null!;

    public string UserMmtype { get; set; } = null!;

    public string UserEngType { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<TblBurmeseRecipe> TblBurmeseRecipes { get; set; } = new List<TblBurmeseRecipe>();
}
