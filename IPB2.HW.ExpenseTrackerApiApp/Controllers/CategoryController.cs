using IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;
using IPB2.HW.ExpenseTrackerApiApp.Models;
using IPB2.HW.ExpenseTrackerApiApp.Models.Category;
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
        public async Task<IActionResult> AddCategory([FromBody] CategoryCreateRequestDTO model)
        {
            var category = new TblCategory
            {
                CategoryName = model.CategoryName,
                Description = model.Description,
                IsDelete = false
            };

            _context.TblCategories.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategoryResponseDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };

            return Ok(new ResponseDTO<CategoryResponseDTO> { Data = result, Message = "Category created successfully." });

        }

        [HttpPut("update-category/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateRequestDTO model)
        {
            var category = await _context.TblCategories
                .FirstOrDefaultAsync(x => x.CategoryId == id && !x.IsDelete);

            if (category == null)
            {
                return NotFound(new ResponseDTO { IsSuccess = false, Message = "Category not found." });
            }

            category.CategoryName = model.CategoryName;
            category.Description = model.Description;

            await _context.SaveChangesAsync();

            var result = new CategoryResponseDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };

            return Ok(new ResponseDTO<CategoryResponseDTO> { Data = result, Message = "Category updated successfully." });
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.TblCategories
                .FirstOrDefaultAsync(x => x.CategoryId == id && !x.IsDelete);

            if (category == null)
            {
                return NotFound(new ResponseDTO { IsSuccess = false, Message = "Category not found." });
            }

            category.IsDelete = true;

            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO { Message = "Category deleted successfully." });
        }

        [HttpGet("list-categories")]
        public async Task<IActionResult> ListCategories()
        {
            var categories = await _context.TblCategories
                .Where(x => !x.IsDelete)
                .Select(x => new CategoryResponseDTO
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    Description = x.Description
                })
                .ToListAsync();

            return Ok(new ResponseDTO<List<CategoryResponseDTO>> { Data = categories, Message = "Categories retrieved successfully." });
        }

    }
}


