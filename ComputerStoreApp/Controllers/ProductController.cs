using ComputerStoreApp.Models.Dtos;
using ComputerStoreApp.Models.Resources;
using ComputerStoreApp.Service;
using ComputerStoreApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStoreApp.Controllers
{
    [ApiController]
    [Route("/api")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<IEnumerable<ProductResource>> GetAllProducts()
        {
            return await _productService.GetAllProductsAsync();
        }

        [HttpGet("product/{productId}")]
        public async Task<ProductResource> GetProductAsync(int productId)
        {
            return await _productService.GetProductAsync(productId);
        }

        [HttpPost("product/add")]
        public async Task<ProductResource> AddProductAsync([FromBody] ProductDto productDto)
        {
            return await _productService.AddProductAsync(productDto);
        }

        [HttpDelete("delete/product/{productId}")]
        public async Task<int> DeleteProduct(int productId)
        {
            return await _productService.DeleteProductAsync(productId);
        }
    }
}
