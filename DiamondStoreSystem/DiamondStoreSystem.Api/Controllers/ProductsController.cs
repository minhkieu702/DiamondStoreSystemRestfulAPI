using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.BusinessLayer.Services;

namespace ProductStoreSystem.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("SearchByCategory")]
        public IActionResult GetByOrigin([FromBody] Dictionary<string, object> grades) => Ok(_productService.FilterList(grades));

        [HttpGet("GetAllWithAllField")]
        public async Task<IActionResult> GetAllWithAllField() => Ok(await _productService.GetAllWithAllField());

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_productService.GetAll().Result);

        [HttpGet("GetByIdWithAllField/{id}")]
        public IActionResult GetByIdWithAllField(string id) => Ok(_productService.IsExist(id).Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id) => Ok(_productService.GetById(id).Result);

        [HttpPost("Create")]
        public IActionResult Create([FromBody] ProductRequestModel model) => Ok(_productService.Create(model).Result);

        [HttpPut("Update/{id}")]
        public IActionResult Update([FromBody] ProductRequestModel model, string id) => Ok(_productService.Update(id, model).Result);

        [HttpPut("Unblock/{id}")]
        public IActionResult UnBlock(string id) => Ok(_productService.UnBlock(id).Result);

        [HttpDelete("Block/{id}")]
        public IActionResult Block(string id) => Ok(_productService.Block(id).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(string id) => Ok(_productService.Delete(id).Result);
    }
}
