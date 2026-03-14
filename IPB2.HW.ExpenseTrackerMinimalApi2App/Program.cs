using IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Budget Module
app.MapPost("/api/Budget/CreateBudget", async (BudgetCreateDTO model, AppDbContext db) =>
{
var exists = await db.TblBudgets
    .AnyAsync(x => x.BudgetYear == model.BudgetYear &&
                   x.BudgetMonth == model.BudgetMonth &&
                   !x.IsDelete);

if (exists)
return Results.BadRequest("Budget already exists for this month.");

var budget = new TblBudget
{
BudgetYear = model.BudgetYear,
BudgetMonth = model.BudgetMonth,
BudgetAmount = model.BudgetAmount,
IsDelete = false
};

db.TblBudgets.Add(budget);
await db.SaveChangesAsync();

return Results.Ok(budget);
})
.WithName("CreateBudget")
.WithOpenApi();

app.MapPut("/api/Budget/UpdateBudget/{id}", async (int id, BudgetUpdateDTO model, AppDbContext db) =>
{
var budget = await db.TblBudgets
    .FirstOrDefaultAsync(x => x.BudgetId == id && !x.IsDelete);

if (budget == null)
return Results.NotFound("Budget not found.");

budget.BudgetYear = model.BudgetYear;
budget.BudgetMonth = model.BudgetMonth;
budget.BudgetAmount = model.BudgetAmount;

await db.SaveChangesAsync();

return Results.Ok(budget);
})
.WithName("UpdateBudget")
.WithOpenApi();

app.MapDelete("/api/Budget/DeleteBudget/{id}", async (int id, AppDbContext db) =>
{
var budget = await db.TblBudgets
    .FirstOrDefaultAsync(x => x.BudgetId == id && !x.IsDelete);

if (budget == null)
return Results.NotFound("Budget not found.");

budget.IsDelete = true;
await db.SaveChangesAsync();

return Results.Ok("Budget deleted successfully.");
})
.WithName("DeleteBudget")
.WithOpenApi();

app.MapGet("/api/Budget/RemainingBudget", async (int year, int month, AppDbContext db) =>
{
var budget = await db.TblBudgets
    .FirstOrDefaultAsync(x => x.BudgetYear == year &&
                              x.BudgetMonth == month &&
                              !x.IsDelete);

if (budget == null)
return Results.NotFound("Budget not found.");

var totalExpense = await db.TblExpenses
    .Where(x => !x.IsDelete &&
                x.ExpenseDate.Year == year &&
                x.ExpenseDate.Month == month)
    .SumAsync(x => (decimal?)x.Amount) ?? 0;

var remaining = budget.BudgetAmount - totalExpense;

var result = new BudgetRemainingDTO
{
BudgetYear = year,
BudgetMonth = month,
BudgetAmount = budget.BudgetAmount,
TotalExpense = totalExpense,
RemainingBudget = remaining
};

return Results.Ok(result);
})
.WithName("RemainingBudget")
.WithOpenApi();

//public class BudgetCreateDTO
//{
//    public int BudgetYear { get; set; }
//    public int BudgetMonth { get; set; }
//    public decimal BudgetAmount { get; set; }
//}

//public class BudgetUpdateDTO
//{
//    public int BudgetYear { get; set; }
//    public int BudgetMonth { get; set; }
//    public decimal BudgetAmount { get; set; }
//}

//public class BudgetRemainingDTO
//{
//    public int BudgetYear { get; set; }
//    public int BudgetMonth { get; set; }
//    public decimal BudgetAmount { get; set; }
//    public decimal TotalExpense { get; set; }
//    public decimal RemainingBudget { get; set; }
//}

//app.MapPut("/api/Category/Update/{id}", async (int id, CategoryUpdateModel model, AppDbContext db) =>
//{
//    var category = await db.TblCategories
//        .FirstOrDefaultAsync(x => x.CategoryId == id && !x.IsDelete);

//    if (category == null)
//        return Results.NotFound("Category not found.");

//    category.CategoryName = model.CategoryName;
//    category.Description = model.Description;

//    await db.SaveChangesAsync();

//    var result = new CategoryDTO
//    {
//        CategoryId = category.CategoryId,
//        CategoryName = category.CategoryName,
//        Description = category.Description
//    };

//    return Results.Ok(result);
//})
//.WithName("UpdateCategory")
//.WithOpenApi();

//app.MapDelete("/api/Category/Delete/{id}", async (int id, AppDbContext db) =>
//{
//    var category = await db.TblCategories
//        .FirstOrDefaultAsync(x => x.CategoryId == id && !x.IsDelete);

//    if (category == null)
//        return Results.NotFound("Category not found.");

//    category.IsDelete = true;
//    await db.SaveChangesAsync();

//    return Results.Ok("Category deleted successfully.");
//})
//.WithName("DeleteCategory")
//.WithOpenApi();

//app.MapGet("/api/Category/List", async (AppDbContext db) =>
//{
//    var categories = await db.TblCategories
//        .Where(x => !x.IsDelete)
//        .Select(x => new CategoryDTO
//        {
//            CategoryId = x.CategoryId,
//            CategoryName = x.CategoryName,
//            Description = x.Description
//        })
//        .ToListAsync();

//    return Results.Ok(categories);
//})
//.WithName("ListCategories")
//.WithOpenApi();

//public class CategoryCreateModel
//{
//    public string CategoryName { get; set; }
//    public string? Description { get; set; }
//}

//public class CategoryUpdateModel
//{
//    public string CategoryName { get; set; }
//    public string? Description { get; set; }
//}

//public class CategoryDTO
//{
//    public int CategoryId { get; set; }
//    public string CategoryName { get; set; }
//    public string? Description { get; set; }
//}

//// Expense
//app.MapPost("/api/Expense/Create", async (ExpenseCreateDTO model, AppDbContext db) =>
//{
//    var expense = new TblExpense
//    {
//        ExpenseDate = model.ExpenseDate,
//        Amount = model.Amount,
//        CategoryId = model.CategoryId,
//        Description = model.Description,
//        IsDelete = false
//    };

//    db.TblExpenses.Add(expense);
//    await db.SaveChangesAsync();

//    var result = new ExpenseDTO
//    {
//        ExpenseId = expense.ExpenseId,
//        ExpenseDate = expense.ExpenseDate,
//        Amount = expense.Amount,
//        CategoryId = expense.CategoryId,
//        CategoryName = expense.Category?.CategoryName,
//        Description = expense.Description
//    };

//    return Results.Ok(result);
//})
//.WithName("CreateExpense")
//.WithOpenApi();

//app.MapPut("/api/Expense/Update/{id}", async (int id, ExpenseUpdateDTO model, AppDbContext db) =>
//{
//    var expense = await db.TblExpenses.FirstOrDefaultAsync(x => x.ExpenseId == id && !x.IsDelete);
//    if (expense == null) return Results.NotFound("Expense not found.");

//    expense.ExpenseDate = model.ExpenseDate;
//    expense.Amount = model.Amount;
//    expense.CategoryId = model.CategoryId;
//    expense.Description = model.Description;

//    await db.SaveChangesAsync();
//    return Results.Ok("Expense updated successfully.");
//})
//.WithName("UpdateExpense")
//.WithOpenApi();

//app.MapDelete("/api/Expense/Delete/{id}", async (int id, AppDbContext db) =>
//{
//    var expense = await db.TblExpenses.FirstOrDefaultAsync(x => x.ExpenseId == id && !x.IsDelete);
//    if (expense == null) return Results.NotFound("Expense not found.");

//    expense.IsDelete = true;
//    await db.SaveChangesAsync();
//    return Results.Ok("Expense deleted successfully.");
//})
//.WithName("DeleteExpense")
//.WithOpenApi();

//app.MapGet("/api/Expense/List", async (AppDbContext db) =>
//{
//    var expenses = await db.TblExpenses
//        .Include(x => x.Category)
//        .Where(x => !x.IsDelete)
//        .Select(x => new ExpenseDTO
//        {
//            ExpenseId = x.ExpenseId,
//            ExpenseDate = x.ExpenseDate,
//            Amount = x.Amount,
//            CategoryId = x.CategoryId,
//            CategoryName = x.Category.CategoryName,
//            Description = x.Description
//        })
//        .ToListAsync();

//    return Results.Ok(expenses);
//})
//.WithName("ListExpenses")
//.WithOpenApi();

//app.MapGet("/api/Expense/Details/{id}", async (int id, AppDbContext db) =>
//{
//    var expense = await db.TblExpenses
//        .Include(x => x.Category)
//        .Where(x => x.ExpenseId == id && !x.IsDelete)
//        .Select(x => new ExpenseDTO
//        {
//            ExpenseId = x.ExpenseId,
//            ExpenseDate = x.ExpenseDate,
//            Amount = x.Amount,
//            CategoryId = x.CategoryId,
//            CategoryName = x.Category.CategoryName,
//            Description = x.Description
//        })
//        .FirstOrDefaultAsync();

//    return expense != null ? Results.Ok(expense) : Results.NotFound("Expense not found.");
//})
//.WithName("ExpenseDetails")
//.WithOpenApi();

//// Summary

//app.MapGet("/api/Expense/MonthlySummary", async (int year, AppDbContext db) =>
//{
//    var query = await db.TblExpenses
//        .Where(x => !x.IsDelete && x.ExpenseDate.Year == year)
//        .GroupBy(x => x.ExpenseDate.Month)
//        .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Amount) })
//        .ToListAsync();

//    var summary = query
//        .Select(x => new
//        {
//            Period = $"{year}-{x.Month:D2}",
//            TotalExpense = x.Total
//        })
//        .OrderBy(x => x.Period)
//        .ToList();

//    return Results.Ok(summary);
//})
//.WithName("MonthlyExpenseSummary")
//.WithOpenApi();

//app.MapGet("/api/Expense/DailySummary", async (int year, int month, AppDbContext db) =>
//{
//    var query = await db.TblExpenses
//        .Where(x => !x.IsDelete && x.ExpenseDate.Year == year && x.ExpenseDate.Month == month)
//        .GroupBy(x => x.ExpenseDate.Date)
//        .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Amount) })
//        .ToListAsync();

//    var dailySummary = query
//        .Select(x => new { Period = x.Date.ToString("yyyy-MM-dd"), TotalExpense = x.Total })
//        .OrderBy(x => x.Period)
//        .ToList();

//    return Results.Ok(dailySummary);
//})
//.WithName("DailyExpenseSummary")
//.WithOpenApi();

//public class ExpenseCreateDTO
//{
//    public DateTime ExpenseDate { get; set; }
//    public decimal Amount { get; set; }
//    public int CategoryId { get; set; }
//    public string? Description { get; set; }
//}

//public class ExpenseUpdateDTO
//{
//    public DateTime ExpenseDate { get; set; }
//    public decimal Amount { get; set; }
//    public int CategoryId { get; set; }
//    public string? Description { get; set; }
//}

//public class ExpenseDTO
//{
//    public int ExpenseId { get; set; }
//    public DateTime ExpenseDate { get; set; }
//    public decimal Amount { get; set; }
//    public int CategoryId { get; set; }
//    public string? CategoryName { get; set; }
//    public string? Description { get; set; }
//}

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
