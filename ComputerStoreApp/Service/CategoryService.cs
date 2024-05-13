using AutoMapper;
using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;
using ComputerStoreApp.Repository.Interface;
using ComputerStoreApp.Service.Interface;

namespace ComputerStoreApp.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public CategoryService(ICategoryRepository categoryRepository,
                               IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResource>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesAsync();
            IEnumerable<CategoryResource> categoryResources = _mapper.Map<List<CategoryResource>>(categories);
            return categoryResources;
        }

        public async Task<CategoryResource> GetCategoryAsync(int categoryId)
        {
            if (categoryId == 0)
            {
                throw new ArgumentException("Please enter a valid category id");
            }

            Category category = await _categoryRepository.GetCategoryAsync(categoryId) ?? throw new ArgumentException($"Category with ID {categoryId} not found.");

            CategoryResource categoryResource = _mapper.Map<CategoryResource>(category);
            return categoryResource;
        }

        public async Task<CategoryResource> AddCategoryAsync(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new Exception(message: "Please enter valid data!");
            }

            Category category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddCategoryAsync(category);
            return _mapper.Map<CategoryResource>(category);
        }

        public async Task<CategoryResource> EditCategoryAsync(int categoryId, CategoryDto categoryDto)
        {
            CategoryResource categoryResourceToUpdate = GetCategoryAsync(categoryId).Result;
            if (categoryResourceToUpdate == null)
            {
                throw new Exception($"Category with ID {categoryId} not found. Please enter existing cateogry id!");
            }
            Category categoryToUpdate = _mapper.Map<Category>(categoryResourceToUpdate);
            categoryToUpdate.CategoryName = categoryDto.CategoryName;
            categoryToUpdate.CategoryDescription = categoryDto.CategoryDescription;

            Category updatedCategory = await _categoryRepository.EditCategoryAsync(categoryToUpdate);

            return _mapper.Map<CategoryResource>(updatedCategory);
        }

        public async Task<CategoryResource> DeleteCategoryAsync(int categoryId)
        {
            Category deletedCategory = await _categoryRepository.DeleteCategoryAsync(categoryId);
            return _mapper.Map<CategoryResource>(deletedCategory);
        }
    }
}
