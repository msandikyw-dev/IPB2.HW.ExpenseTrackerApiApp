using IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;
using IPB2.HW.ExpenseTrackerApiApp.Models;
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
        public async Task<IActionResult> CreateMonthlyBudget([FromBody] BudgetCreateRequestDTO model)
        {
            var exists = await _context.TblBudgets
                .AnyAsync(x => x.BudgetYear == model.BudgetYear &&
                               x.BudgetMonth == model.BudgetMonth &&
                               !x.IsDelete);

            if (exists)
            {
                return BadRequest(new ResponseDTO { IsSuccess = false, Message = "Budget already exists for this month." });
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

            var result = new BudgetResponseDTO
            {
                BudgetId = budget.BudgetId,
                BudgetYear = budget.BudgetYear,
                BudgetMonth = budget.BudgetMonth,
                BudgetAmount = budget.BudgetAmount
            };

            return Ok(new ResponseDTO<BudgetResponseDTO> { Data = result, Message = "Budget created successfully." });
        }

        [HttpPut("update-budget/{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] BudgetUpdateRequestDTO model)
        {
            var budget = await _context.TblBudgets
                .FirstOrDefaultAsync(x => x.BudgetId == id && !x.IsDelete);

            if (budget == null)
            {
                return NotFound(new ResponseDTO { IsSuccess = false, Message = "Budget not found." });
            }

            budget.BudgetYear = model.BudgetYear;
            budget.BudgetMonth = model.BudgetMonth;
            budget.BudgetAmount = model.BudgetAmount;

            await _context.SaveChangesAsync();

            var result = new BudgetResponseDTO
            {
                BudgetId = budget.BudgetId,
                BudgetYear = budget.BudgetYear,
                BudgetMonth = budget.BudgetMonth,
                BudgetAmount = budget.BudgetAmount
            };

            return Ok(new ResponseDTO<BudgetResponseDTO> { Data = result, Message = "Budget updated successfully." });
        }

        [HttpDelete("delete-budget/{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.TblBudgets
                .FirstOrDefaultAsync(x => x.BudgetId == id && !x.IsDelete);

            if (budget == null)
            {
                return NotFound(new ResponseDTO { IsSuccess = false, Message = "Budget not found." });
            }

            budget.IsDelete = true;

            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO { Message = "Budget deleted successfully." });
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
                return NotFound(new ResponseDTO { IsSuccess = false, Message = "Budget not found for this month." });
            }

            var totalExpense = await _context.TblExpenses
                .Where(x => x.ExpenseDate.Year == year &&
                            x.ExpenseDate.Month == month)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            var remaining = budget.BudgetAmount - totalExpense;

            var result = new BudgetRemainingResponseDTO
            {
                BudgetYear = year,
                BudgetMonth = month,
                BudgetAmount = budget.BudgetAmount,
                TotalExpense = totalExpense,
                RemainingBudget = remaining
            };

            return Ok(new ResponseDTO<BudgetRemainingResponseDTO> { Data = result, Message = "Remaining budget retrieved successfully." });
        }
    }
}

public class BudgetCreateRequestDTO
{
    public int BudgetYear { get; set; }

    public int BudgetMonth { get; set; }

    public decimal BudgetAmount { get; set; }
}

public class BudgetUpdateRequestDTO
{
    public int BudgetYear { get; set; }

    public int BudgetMonth { get; set; }

    public decimal BudgetAmount { get; set; }
}

public class BudgetResponseDTO
{
    public int BudgetId { get; set; }
    public int BudgetYear { get; set; }

    public int BudgetMonth { get; set; }

    public decimal BudgetAmount { get; set; }
}

public class BudgetRemainingResponseDTO
{
    public int BudgetYear { get; set; }

    public int BudgetMonth { get; set; }

    public decimal BudgetAmount { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal RemainingBudget { get; set; }
}
