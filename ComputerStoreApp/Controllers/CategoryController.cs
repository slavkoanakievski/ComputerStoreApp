using ComputerStoreApp.Models;
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
        public Task<IEnumerable<CategoryResource>> GetAllCategories()
        {
           return _categoryService.GetAllCategoriesAsync();
        }
    }
}
