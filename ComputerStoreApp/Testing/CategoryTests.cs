using ComputerStoreApp.Models;
using ComputerStoreApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace ComputerStoreApp.Testing
{
    [TestClass]
    public class CategoryTests
    {
        private static DbContextOptions<CoreDbContext> _options;
        private static List<Category> _categories;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _options = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Laptops" },
                new Category { CategoryId = 2, CategoryName = "Desktops" }
            };

            using (var dbContext = new CoreDbContext(_options))
            {
                dbContext.Categories.AddRange(_categories);
                dbContext.SaveChanges();
            }
        }

        #region GetAllCategoriesAsync

        [TestMethod]
        public async Task GetAllCategoriesAsync_ReturnsListOfCategories()
        {
            using (var context = new CoreDbContext(_options))
            {
                var repository = new CategoryRepository(context);

                var result = (List<Category>)await repository.GetAllCategoriesAsync();

                //this one will be added on AddCategoryAsync test
                //_categories.Add(new Category { CategoryId = 3, CategoryName = "Tablets" });

                CollectionAssert.AreEquivalent(result, _categories);
                CollectionAssert.AllItemsAreNotNull(result);
            }
        }

        #endregion

        #region GetCategoryAsync

        [TestMethod]
        public async Task GetCategoryAsync_ReturnsCategory()
        {
            using (var context = new CoreDbContext(_options))
            {
                var repository = new CategoryRepository(context);

                var result = await repository.GetCategoryAsync(1);

                Assert.IsNotNull(result);
                Assert.AreEqual(_categories[0], result);
            }
        }

        #endregion


        #region AddCategoryAsync

        [TestMethod]
        public async Task AddCategoryAsync_ReturnsAddedCategory()
        {
            // Arrange
            var categoryToAdd = new Category { CategoryId = 3, CategoryName = "Tablets" };

            // Execute the method being tested
            using (var context = new CoreDbContext(_options))
            {
                var repository = new CategoryRepository(context);

                // Act
                var result = await repository.AddCategoryAsync(categoryToAdd);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(categoryToAdd.CategoryId, result.CategoryId);
                Assert.AreEqual(categoryToAdd.CategoryName, result.CategoryName);
            }
        }

        #endregion

        #region EditCategoryAsync

        [TestMethod]
        public async Task EditCategoryAsync_ReturnsEditedCategory()
        {
            var categoryToEdit = _categories[0];
            categoryToEdit.CategoryName = "Updated Laptop Category";

            using (var context = new CoreDbContext(_options))
            {
                var repository = new CategoryRepository(context);

                // Act
                var result = await repository.EditCategoryAsync(categoryToEdit);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(categoryToEdit.CategoryId, result.CategoryId);
                Assert.AreEqual(categoryToEdit.CategoryName, result.CategoryName);
            }
        }

        #endregion


        #region DeleteCategoryAsync

        [TestMethod]
        public async Task DeleteCategoryAsync_RemovesCategoryFromDatabase()
        {
            int categoryIdToDelete = 3;
            using (var context = new CoreDbContext(_options))
            {
                var repository = new CategoryRepository(context);
                var initialCategoryCount = await context.Categories.CountAsync();

                var deletedCategory = await repository.DeleteCategoryAsync(categoryIdToDelete);

                Assert.IsNotNull(deletedCategory);
                Assert.AreEqual(categoryIdToDelete, deletedCategory.CategoryId);

                var remainingCategory = await repository.GetCategoryAsync(categoryIdToDelete);
                Assert.IsNull(remainingCategory);

                var finalCategoryCount = await context.Categories.CountAsync();
                Assert.AreEqual(initialCategoryCount - 1, finalCategoryCount);
            }
        }

        #endregion




    }
}
