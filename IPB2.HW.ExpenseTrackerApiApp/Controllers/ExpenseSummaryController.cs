using IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;
using IPB2.HW.ExpenseTrackerApiApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HW.ExpenseTrackerApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseSummaryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ExpenseSummaryController()
        {
            _context = new AppDbContext();
        }

        [HttpGet("monthly-expenses")]
        public async Task<IActionResult> GetMonthlyExpenses(int year)
        {
            var query = await _context.TblExpenses
                .Where(x => !x.IsDelete && x.ExpenseDate.Year == year)
                .GroupBy(x => x.ExpenseDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            var summary = query
                .Select(x => new ExpenseSummaryResponseDTO
                {
                    Period = $"{year}-{x.Month:D2}",
                    TotalExpense = x.Total
                })
                .OrderBy(x => x.Period)
                .ToList();

            return Ok(new ResponseDTO<List<ExpenseSummaryResponseDTO>> { Data = summary, Message = "Monthly expenses retrieved successfully." });
        }

        [HttpGet("daily-expenses")]
        public async Task<IActionResult> GetDailyExpenses(int year, int month)
        {
            var query = await _context.TblExpenses
        .Where(x => !x.IsDelete &&
                    x.ExpenseDate.Year == year &&
                    x.ExpenseDate.Month == month)
        .GroupBy(x => x.ExpenseDate.Date)
        .Select(g => new
        {
            Date = g.Key,               
            Total = g.Sum(x => x.Amount)
        })
        .ToListAsync();               

        var dailySummary = query
            .Select(x => new ExpenseSummaryResponseDTO
            {
                Period = x.Date.ToString("yyyy-MM-dd"),
                TotalExpense = x.Total
            })
            .OrderBy(x => x.Period)
            .ToList();

        return Ok(new ResponseDTO<List<ExpenseSummaryResponseDTO>> { Data = dailySummary, Message = "Daily expenses retrieved successfully." });
        }
    }
}

public class ExpenseSummaryResponseDTO
{
    public string Period { get; set; }  // e.g., "2026-03" or "2026-W10" or "2026-03-12"
    public decimal TotalExpense { get; set; }
}
