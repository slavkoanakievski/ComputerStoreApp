using AutoMapper;
using ComputerStoreApp.Models;
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
    }
}
