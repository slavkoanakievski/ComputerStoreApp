using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;
using ComputerStoreApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStoreApp.Controllers
{
    [ApiController]
    [Route("/api")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger,
                                  ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("categories")]
        public async Task<IEnumerable<CategoryResource>> GetAllCategories()
        {
            return await _categoryService.GetAllCategoriesAsync();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<CategoryResource> GetCategoryAsync(int categoryId)
        {
            return await _categoryService.GetCategoryAsync(categoryId);
        }

        [HttpPost("category/add")]
        public async Task<CategoryResource> AddCategory([FromBody] CategoryDto categoryDto)
        {
            return await _categoryService.AddCategoryAsync(categoryDto);
        }

        [HttpPatch("edit/{categoryId}")]
        public async Task<CategoryResource> EditCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            return await _categoryService.EditCategoryAsync(categoryId, categoryDto);
        }

        [HttpDelete("delete/category/{categoryId}")]
        public async Task<CategoryResource> DeleteCategory(int categoryId)
        {
            return await _categoryService.DeleteCategoryAsync(categoryId);
        }

    }
}
