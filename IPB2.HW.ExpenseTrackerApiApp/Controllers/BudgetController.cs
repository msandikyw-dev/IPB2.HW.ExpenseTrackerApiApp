using IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HW.ExpenseTrackerApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BudgetController()
        {
            _context = new AppDbContext();
        }

        [HttpPost("create-budget")]
        public async Task<IActionResult> CreateMonthlyBudget([FromBody] BudgetCreateDTO model)
        {
            var exists = await _context.TblBudgets
                .AnyAsync(x => x.BudgetYear == model.BudgetYear &&
                               x.BudgetMonth == model.BudgetMonth &&
                               !x.IsDelete);

            if (exists)
            {
                return BadRequest("Budget already exists for this month.");
            }

            var budget = new TblBudget
            {
                BudgetYear = model.BudgetYear,
                BudgetMonth = model.BudgetMonth,
                BudgetAmount = model.BudgetAmount,
                IsDelete = false
            };

            _context.TblBudgets.Add(budget);
            await _context.SaveChangesAsync();

            return Ok(budget);
        }

        [HttpPut("update-budget/{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] BudgetUpdateDTO model)
        {
            var budget = await _context.TblBudgets
                .FirstOrDefaultAsync(x => x.BudgetId == id && !x.IsDelete);

            if (budget == null)
            {
                return NotFound("Budget not found.");
            }

            budget.BudgetYear = model.BudgetYear;
            budget.BudgetMonth = model.BudgetMonth;
            budget.BudgetAmount = model.BudgetAmount;

            await _context.SaveChangesAsync();

            return Ok(budget);
        }

        [HttpDelete("delete-budget/{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.TblBudgets
                .FirstOrDefaultAsync(x => x.BudgetId == id && !x.IsDelete);

            if (budget == null)
            {
                return NotFound("Budget not found.");
            }

            budget.IsDelete = true;

            await _context.SaveChangesAsync();

            return Ok("Budget deleted successfully.");
        }

        [HttpGet("remaining-budget")]
        public async Task<IActionResult> GetRemainingBudget(int year, int month)
        {
            var budget = await _context.TblBudgets
                .FirstOrDefaultAsync(x =>
                    x.BudgetYear == year &&
                    x.BudgetMonth == month &&
                    !x.IsDelete);

            if (budget == null)
            {
                return NotFound("Budget not found.");
            }

            var totalExpense = await _context.TblExpenses
                .Where(x => x.ExpenseDate.Year == year &&
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

            return Ok(null);
        }
    }
}

public class BudgetCreateDTO
{
    public int BudgetYear { get; set; }

    public int BudgetMonth { get; set; }

    public decimal BudgetAmount { get; set; }
}

public class BudgetUpdateDTO
{
    public int BudgetYear { get; set; }

    public int BudgetMonth { get; set; }

    public decimal BudgetAmount { get; set; }
}

public class BudgetRemainingDTO
{
    public int BudgetYear { get; set; }

    public int BudgetMonth { get; set; }

    public decimal BudgetAmount { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal RemainingBudget { get; set; }
}


