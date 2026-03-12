using IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HW.ExpenseTrackerApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseController()
        {
            _context = new AppDbContext();
        }

        [HttpPost("create-expense")]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseCreateDTO model)
        {
            var expense = new TblExpense
            {
                ExpenseDate = model.ExpenseDate,
                Amount = model.Amount,
                CategoryId = model.CategoryId,
                Description = model.Description,
                IsDelete = false  
            };

            _context.TblExpenses.Add(expense);
            await _context.SaveChangesAsync();

            var result = new ExpenseDTO
            {
                ExpenseId = expense.ExpenseId,
                ExpenseDate = expense.ExpenseDate,
                Amount = expense.Amount,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category?.CategoryName,
                Description = expense.Description
            };

            return Ok(result);
        }

        [HttpPut("update-expense/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseUpdateDTO model)
        {
            var expense = await _context.TblExpenses
            .FirstOrDefaultAsync(x => x.ExpenseId == id && !x.IsDelete);

            if (expense == null)
            {
                return NotFound("Expense not found.");
            }

            expense.ExpenseDate = model.ExpenseDate;
            expense.Amount = model.Amount;
            expense.CategoryId = model.CategoryId;
            expense.Description = model.Description;

            await _context.SaveChangesAsync();

            return Ok("Expense updated successfully.");
        }

        [HttpDelete("delete-expense/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.TblExpenses
            .FirstOrDefaultAsync(x => x.ExpenseId == id && !x.IsDelete);

            if (expense == null)
            {
                return NotFound("Expense not found.");
            }

            expense.IsDelete = true;

            await _context.SaveChangesAsync();

            return Ok("Expense deleted successfully.");
        }

        [HttpGet("list-expenses")]
        public async Task<IActionResult> ListExpenses()
        {
            var expenses = await _context.TblExpenses
            .Include(x => x.Category)
            .Where(x => !x.IsDelete)
            .Select(x => new ExpenseDTO
            {
                ExpenseId = x.ExpenseId,
                ExpenseDate = x.ExpenseDate,
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Description = x.Description
            })
            .ToListAsync();

            return Ok(expenses);
        }

        [HttpGet("expense-details/{id}")]
        public async Task<IActionResult> ExpenseDetails(int id)
        {
            var expense = await _context.TblExpenses
           .Include(x => x.Category)
           .Where(x => x.ExpenseId == id && !x.IsDelete) 
           .Select(x => new ExpenseDTO
           {
               ExpenseId = x.ExpenseId,
               ExpenseDate = x.ExpenseDate,
               Amount = x.Amount,
               CategoryId = x.CategoryId,
               CategoryName = x.Category.CategoryName,
               Description = x.Description
           })
           .FirstOrDefaultAsync();

            if (expense == null)
            {
                return NotFound("Expense not found.");
            }

            return Ok(expense);
        }

    }
}

public class ExpenseCreateDTO
{
    public DateTime ExpenseDate { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }
}

public class ExpenseUpdateDTO
{
    public DateTime ExpenseDate { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }
}

public class ExpenseDTO
{
    public int ExpenseId { get; set; }

    public DateTime ExpenseDate { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Description { get; set; }
}
