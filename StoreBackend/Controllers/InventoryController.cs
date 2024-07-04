using Microsoft.AspNetCore.Mvc;
using StoreService.Context;
using StoreService.Interface;
using StoreService.Service;

namespace StoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromQuery]string categoryName)
        {
            return Ok( new
            {
                Id = await _inventoryService.AddCategory(categoryName)
            });
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _inventoryService.GetCategories());
        }
    }
}
