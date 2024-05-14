using AutoMapper;
using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;
using ComputerStoreApp.Repository;
using ComputerStoreApp.Repository.Interface;
using ComputerStoreApp.Service.Interface;

namespace ComputerStoreApp.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IProductCategoryRepository productCategoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResource>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _productRepository.GetAllProductsAsync();
            IEnumerable<ProductResource> productResources = products.Select(p => new ProductResource
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductPrice = p.ProductPrice,
                ProductStock = p.ProductStock,
                Categories = p.ProductCategories.Select(pc =>
                {
                    Category cat = _categoryRepository.GetCategoryAsync(pc.CategoryId).Result;
                    return cat != null ? new CategoryResource
                    {
                        CategoryId = pc.CategoryId,
                        CategoryName = cat.CategoryName,
                        CategoryDescription = cat.CategoryDescription
                    } : null;
                }).ToList()
            });
            return productResources;
        }

        public async Task<ProductResource> GetProductAsync(int productId)
        {
            Product product = await _productRepository.GetProductAsync(productId);
            if (product == null)
            {
                throw new Exception(message: "There is no product with id: " + productId);
            }

            ProductResource productResource = new ProductResource
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ProductStock = product.ProductStock,
                Categories = product.ProductCategories.Select(pc =>
                {
                    Category cat = _categoryRepository.GetCategoryAsync(pc.CategoryId).Result;
                    return cat != null ? new CategoryResource
                    {
                        CategoryId = pc.CategoryId,
                        CategoryName = cat.CategoryName,
                        CategoryDescription = cat.CategoryDescription
                    } : null;
                }).ToList()
            };
            return productResource;
        }

        public async Task<ProductResource> AddProductAsync(ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new Exception(message: "Invalid product data");
            }

            List<Category> categories = productDto.Categories
                        .Select(categoryId => _categoryRepository.GetCategoryAsync(categoryId).Result)
                        .ToList();


            Product newProduct = new Product
            {
                ProductName = productDto.ProductName,
                ProductDescription = productDto.ProductDescription,
                ProductPrice = productDto.ProductPrice,
                ProductStock = productDto.ProductStock,
            };

            var addedProduct = await _productRepository.AddProductAsync(newProduct);

            List<ProductCategory> productCategories = categories
                .Select(c => new ProductCategory
                {
                    ProductId = addedProduct.ProductId,
                    CategoryId = c.CategoryId,
                    //Category = c,
                    //Product = addedProduct
                }).ToList();

            await _productCategoryRepository.AddProductCategoriesAsync(productCategories);

            return new ProductResource
            {
                ProductId = addedProduct.ProductId,
                ProductName = addedProduct.ProductName,
                ProductDescription = addedProduct.ProductDescription,
                ProductPrice = addedProduct.ProductPrice,
                ProductStock = addedProduct.ProductStock,
                Categories = productCategories.Select(pc =>
                {
                    Category cat = _categoryRepository.GetCategoryAsync(pc.CategoryId).Result;
                    return cat != null ? new CategoryResource
                    {
                        CategoryId = pc.CategoryId,
                        CategoryName = cat.CategoryName,
                        CategoryDescription = cat.CategoryDescription
                    } : null;
                }).ToList()
            };
        }

        public async Task<int> DeleteProductAsync(int productId)
        {
            Product product = await _productRepository.GetProductAsync(productId);
            if(product  == null)
            {
                throw new Exception(message: "There is no product with id: " + productId);
            }

            List<int> categories = product.ProductCategories
                .Select(pc => pc.CategoryId)
                .ToList();

            List<ProductCategory> productCategories  = new List<ProductCategory>();

            foreach (int categoryId in categories) {
                ProductCategory productCategory = await _productCategoryRepository
                    .GetProductCategory(productId, categoryId);
                productCategories.Add(productCategory);
            }

            await _productCategoryRepository.DeleteProductCategories(productCategories);

            await _productRepository.DeleteProductAsync(productId);
            return productId;
        }
    }
}
