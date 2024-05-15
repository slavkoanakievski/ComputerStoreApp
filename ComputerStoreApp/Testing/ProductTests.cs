using ComputerStoreApp.Models;
using ComputerStoreApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputerStoreApp.Testing
{
    [TestClass]
    public class ProductTests
    {
        private static DbContextOptions<CoreDbContext> _options;
        private static List<Product> _products;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "Laptop", ProductPrice = 999, ProductStock = 10 },
            new Product { ProductId = 2, ProductName = "Desktop", ProductPrice = 799, ProductStock = 20 }
        };

            using (var dbContext = new CoreDbContext(_options))
            {
                dbContext.Products.AddRange(_products);
                dbContext.SaveChanges();
            }
        }


        #region GetAllProductsAsync

        [TestMethod]
        public async Task GetAllProductsAsync_ReturnsListOfProducts()
        {
            using (var context = new CoreDbContext(_options))
            {
                var repository = new ProductRepository(context);

                var result = (List<Product>)await repository.GetAllProductsAsync();

                CollectionAssert.AllItemsAreNotNull(result);
            }
        }

        #endregion

        #region GetProductAsync

        [TestMethod]
        public async Task GetProductAsync_ReturnsProduct()
        {
            using (var context = new CoreDbContext(_options))
            {
                var repository = new ProductRepository(context);

                var result = await repository.GetProductAsync(1);

                Assert.IsNotNull(result);
                Assert.AreEqual(_products[0], result);
            }
        }

        [TestMethod]
        public async Task GetProductByNameAsync_ReturnsProduct()
        {
            // Arrange
            string productName = "Laptop";

            using (var context = new CoreDbContext(_options))
            {
                var repository = new ProductRepository(context);

                // Act
                var result = await repository.GetProductByNameAsync(productName);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(productName, result.ProductName);
            }
        }

        #endregion


        #region AddProductAsync
        [TestMethod]
        public async Task AddProductAsync_ReturnsAddedProduct()
        {
            var productToAdd = new Product { ProductId = 3, ProductName = "Tablet", ProductPrice = 399.99m, ProductStock = 15 };

            using (var context = new CoreDbContext(_options))
            {
                var repository = new ProductRepository(context);

                var result = await repository.AddProductAsync(productToAdd);

                Assert.IsNotNull(result);
                Assert.AreEqual(productToAdd.ProductId, result.ProductId);
                Assert.AreEqual(productToAdd.ProductName, result.ProductName);
                // Add more assertions as needed
            }
        }

        #endregion

        #region DeleteProductAsync

        [TestMethod]
        public async Task DeleteProductAsync_RemovesProductFromDatabase()
        {
            int productIdToDelete = 3;

            using (var context = new CoreDbContext(_options))
            {
                var repository = new ProductRepository(context);
                var initialProductCount = await context.Products.CountAsync();

                var deletedProduct = await repository.DeleteProductAsync(productIdToDelete);

                Assert.IsNotNull(deletedProduct);
                Assert.AreEqual(productIdToDelete, deletedProduct.ProductId);

                var remainingProduct = await repository.GetProductAsync(productIdToDelete);
                Assert.IsNull(remainingProduct);

                var finalProductCount = await context.Products.CountAsync();
                Assert.AreEqual(initialProductCount - 1, finalProductCount);
            }
        }
        #endregion

        #region GetProductStocksAsync
        [TestMethod]
        public async Task GetProductStocksAsync_ReturnsProductStocks()
        {
            var productIds = new List<int> { 1, 2 };

            using (var context = new CoreDbContext(_options))
            {
                var repository = new ProductRepository(context);

                var result = await repository.GetProductStocksAsync(productIds);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(10, result[1]);
                Assert.AreEqual(20, result[2]);
            }
        }
    }

    #endregion


}
