using IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HW.ExpenseTrackerApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController()
        {
            _context = new AppDbContext();
        }

        [HttpPost("create-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryCreateModel model)
        {
            var category = new TblCategory
            {
                CategoryName = model.CategoryName,
                Description = model.Description,
                IsDelete = false
            };

            _context.TblCategories.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };

            return Ok(result);

        }

        [HttpPut("update-category/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateModel model)
        {
            var category = await _context.TblCategories
                .FirstOrDefaultAsync(x => x.CategoryId == id && !x.IsDelete);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            category.CategoryName = model.CategoryName;
            category.Description = model.Description;

            await _context.SaveChangesAsync();

            var result = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };

            return Ok(result);
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.TblCategories
                .FirstOrDefaultAsync(x => x.CategoryId == id && !x.IsDelete);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            category.IsDelete = true;

            await _context.SaveChangesAsync();

            return Ok("Category deleted successfully.");
        }

        [HttpGet("list-categories")]
        public async Task<IActionResult> ListCategories()
        {
            var categories = await _context.TblCategories
                .Where(x => !x.IsDelete)
                .Select(x => new CategoryDTO
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    Description = x.Description
                })
                .ToListAsync();

            return Ok(categories);
        }

    }
}

public class CategoryCreateModel
{
    public string CategoryName { get; set; }

    public string? Description { get; set; }
}

public class CategoryUpdateModel
{
    public string CategoryName { get; set; }

    public string? Description { get; set; }
}

public class CategoryDTO
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string? Description { get; set; }
}
