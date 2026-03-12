using IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;
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

        //[HttpGet("monthly-expenses")]
        //public async Task<IActionResult> GetMonthlyExpenses(int year)
        //{
        //    var summary = await _context.TblExpenses
        //        .Where(x => !x.IsDelete && x.ExpenseDate.Year == year)
        //        .GroupBy(x => x.ExpenseDate.Month)
        //        .Select(g => new ExpenseSummaryDTO
        //        {
        //            Period = $"{year}-{g.Key:D2}",  // e.g., "2026-03"
        //            TotalExpense = g.Sum(x => x.Amount)
        //        })
        //        .OrderBy(x => x.Period)
        //        .ToListAsync();

        //    return Ok(summary);
        //}

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
                .Select(x => new
                {
                    Period = $"{year}-{x.Month:D2}",
                    TotalExpense = x.Total
                })
                .OrderBy(x => x.Period)
                .ToList();

            return Ok(summary);
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
            .Select(x => new
            {
                Period = x.Date.ToString("yyyy-MM-dd"),
                TotalExpense = x.Total
            })
            .OrderBy(x => x.Period)
            .ToList();

        return Ok(dailySummary);
        }
    }
}

public class ExpenseSummaryDTO
{
    public string Period { get; set; }  // e.g., "2026-03" or "2026-W10" or "2026-03-12"
    public decimal TotalExpense { get; set; }
}
