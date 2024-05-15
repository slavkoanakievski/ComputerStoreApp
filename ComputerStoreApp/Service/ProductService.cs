using AutoMapper;
using ComputerStoreApp.Models;
using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;
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
            if (product == null)
            {
                throw new Exception(message: "There is no product with id: " + productId);
            }

            List<int> categories = product.ProductCategories
                .Select(pc => pc.CategoryId)
                .ToList();

            List<ProductCategory> productCategories = new List<ProductCategory>();

            foreach (int categoryId in categories)
            {
                ProductCategory productCategory = await _productCategoryRepository
                    .GetProductCategory(productId, categoryId);
                productCategories.Add(productCategory);
            }

            await _productCategoryRepository.DeleteProductCategories(productCategories);

            await _productRepository.DeleteProductAsync(productId);
            return productId;
        }

        public async Task<ProductResource> UpdateProductAsync(int productId, ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new Exception(message: "Invalid product data");
            }

            var existingProduct = await _productRepository.GetProductAsync(productId);

            if (existingProduct == null)
            {
                throw new Exception(message: $"Product with id {productId} not found");
            }

            existingProduct.ProductName = productDto.ProductName;
            existingProduct.ProductDescription = productDto.ProductDescription;
            existingProduct.ProductPrice = productDto.ProductPrice;
            existingProduct.ProductStock = productDto.ProductStock;

            await _productRepository.UpdateProductAsync(existingProduct);

            await UpdateProductCategoriesAsync(productId, productDto.Categories);

            return await GetProductResourceAsync(productId);
        }


        private async Task UpdateProductCategoriesAsync(int productId, IEnumerable<int> categoryIds)
        {
            Product product = await _productRepository.GetProductAsync(productId);
            List<int> existingCategories = product.ProductCategories.Select(pc => pc.CategoryId).ToList();
            List<int> categoriesToAdd = categoryIds.Except(existingCategories).ToList();

            var categoriesToRemove = existingCategories.Where(categoryId => !categoryIds.Contains(categoryId)).ToList();
            //List<ProductCategory> productCategories = new List<ProductCategory>();

            //foreach (int catId in categoriesToRemove)
            //{
            //    ProductCategory productCategory = await _productCategoryRepository
            //        .GetProductCategory(productId, catId);
            //    productCategories.Add(productCategory);
            //}

            //await _productCategoryRepository.DeleteProductCategories(productCategories);

            var newProductCategories = categoriesToAdd.Select(c => new ProductCategory
            {
                ProductId = productId,
                CategoryId = c
            }).ToList();

            await _productCategoryRepository.AddProductCategoriesAsync(newProductCategories);
        }

        private async Task<ProductResource> GetProductResourceAsync(int productId)
        {
            var product = await _productRepository.GetProductAsync(productId);

            if (product == null)
            {
                return null;
            }

            List<CategoryResource> categories = product.ProductCategories.Select(pc =>
            {
                Category cat = _categoryRepository.GetCategoryAsync(pc.CategoryId).Result;
                return cat != null ? new CategoryResource
                {
                    CategoryId = pc.CategoryId,
                    CategoryName = cat.CategoryName,
                    CategoryDescription = cat.CategoryDescription
                } : null;
            }).ToList();

            return new ProductResource
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ProductStock = product.ProductStock,
                Categories = categories
            };
        }

        public async Task UpdateProductStock(List<StockInfoDto> stockInfoDtos)
        {
            foreach (StockInfoDto stockInfoDto in stockInfoDtos)
            {
                List<Category> addedCategories = await AddMissingCategoies(stockInfoDto);
                Product product = await AddOrUpdateProductAsync(stockInfoDto);

                List<ProductCategory> productCategories = addedCategories
                .Select(c => new ProductCategory
                {
                    ProductId = product.ProductId,
                    CategoryId = c.CategoryId,
                    //Category = c,
                    //Product = addedProduct
                }).ToList();

                await _productCategoryRepository.AddProductCategoriesAsync(productCategories);
            }
        }

        private async Task<List<Category>> AddMissingCategoies(StockInfoDto stockInfoDto)
        {
            IEnumerable<string> categoryNames = stockInfoDto?.CategoryNames;
            IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesAsync();
            IEnumerable<string> existingCategoryNames = categories.Select(c => c.CategoryName);

            IEnumerable<string> categoryNamesToAdd = categoryNames?.Except(existingCategoryNames).ToList();

            List<Category> addedCategories = new List<Category>();

            foreach (string categoryName in categoryNamesToAdd)
            {
                Category newCat = await _categoryRepository.AddCategoryAsync(new Category
                {
                    CategoryName = categoryName
                });

                addedCategories.Add(newCat);

            }
            return addedCategories;
        }

        private async Task<Product> AddOrUpdateProductAsync(StockInfoDto stockInfoDto)
        {
            Product existingProduct = await _productRepository.GetProductByNameAsync(stockInfoDto.Name);

            if (existingProduct == null)
            {
                return await _productRepository.AddProductAsync(new Product
                {
                    ProductName = stockInfoDto.Name,
                    ProductPrice = stockInfoDto.Price,
                    ProductStock = stockInfoDto.Quantity
                });
            }
            else
            {
                existingProduct.ProductStock += stockInfoDto.Quantity;
                await _productRepository.UpdateProductAsync(existingProduct);
                return existingProduct;
            }
        }

        public async Task<CalculatedDiscountDto> CalculateDiscountForItems(List<PurchasedOrderItemDto> purchasedOrderItemDtos)
        {
            await ValidateProductStock(purchasedOrderItemDtos);

            List<Product> products = new List<Product>();
            //IEnumerable<Task<Product>> productTasks = purchasedOrderItemDtos.Select(async item => await _productRepository.GetProductAsync(item.ProductId));
            //var products = await Task.WhenAll(productTasks);

            foreach (var item in purchasedOrderItemDtos)
            {
                Product product = await _productRepository.GetProductAsync(item.ProductId);
                products.Add(product);
            }

            var itemsByCategory = products
                .SelectMany(item => item.ProductCategories, async (item, category) => new { Product = await _productRepository.GetProductAsync(item.ProductId), CategoryId = category.CategoryId })
                .GroupBy(item => item.Result.CategoryId);

            double totalDiscount = 0;
            double totalPrice = 0;
            int numberOfDiscountedProducts = 0;

            foreach (var itemGroup in itemsByCategory)
            {
                var firstItem = itemGroup.First();
                double discount = 0;

                if (itemGroup.Count() > 1)
                {
                    var firstProductItem = purchasedOrderItemDtos.Where(p => p.ProductId == firstItem.Result.Product.ProductId).FirstOrDefault();
                    discount = (double)firstProductItem.Price * 0.05;
                    totalPrice = totalPrice + (firstProductItem.Price * firstProductItem.Quantity);
                    numberOfDiscountedProducts++;
                }

                foreach (var item in itemGroup.Skip(1))
                {
                    item.Result.Product.ProductPrice = (decimal)((double)item.Result.Product.ProductPrice - discount);
                    var productFromCart = purchasedOrderItemDtos.Where(p => p.ProductId == item.Result.Product.ProductId).FirstOrDefault();
                    totalPrice = totalPrice + ((double)productFromCart.Price * productFromCart.Quantity);
                }

                totalDiscount += discount; //* (itemGroup.Count() - 1);
            }

            return new CalculatedDiscountDto
            {
                Discount = totalDiscount,
                OrderTotalPrice = totalPrice,
                OrderDiscountedPrice = totalPrice - totalDiscount,
                NumberOfDiscountedProducts = numberOfDiscountedProducts
            };

        }

        private async Task ValidateProductStock(List<PurchasedOrderItemDto> purchasedOrderItemDtos)
        {
            List<int> productIds = purchasedOrderItemDtos.Select(p => p.ProductId).ToList();
            Dictionary<int, int> productStocks = await _productRepository.GetProductStocksAsync(productIds);

            foreach (var item in purchasedOrderItemDtos)
            {
                var availableStock = productStocks.FirstOrDefault(s => s.Key == item.ProductId).Value;
                if (availableStock < item.Quantity)
                {
                    throw new Exception($"Insufficient stock for product with id: {item.ProductId}");
                }
            }
        }
    }
}
