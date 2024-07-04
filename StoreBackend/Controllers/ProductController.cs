using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreService.Context;
using StoreService.EntityModels;
using StoreService.Interface;
using StoreService.Models;

namespace StoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _productService.GetProducts()); 
        }

        [HttpPost("PostProducts")]
        public async Task<ActionResult<Product>> PostProduct(NewProduct product)
        {
            return Ok(new
            {
                Id = await _productService.PostProduct(product)
            });
        }

        [HttpGet("GetProductDetails")]
        public async Task<IActionResult> GetProductDetails([FromQuery] List<Guid> productIds)
        {
            return Ok(await _productService.GetProductDetails(productIds));
        }

        [HttpGet("SearchProducts")]
        public async Task<IActionResult> SearchProducts([FromQuery] string productName)
        {
            return Ok(await _productService.SearchProducts(productName));
        }

        [HttpPost("MarkOutOfStock")]
        public async Task<IActionResult> MarkOutOfStock(Guid productId)
        {
            return Ok(await _productService.MarkOutOfStock(productId));
        }

        [HttpPost("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(ProductRequest product)
        {
            return Ok(await _productService.UpdateQuantity(product));
        }

        [HttpPost("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice(ProductRequest product)
        {
            return Ok(await _productService.UpdatePrice(product));
        }
    }
}
