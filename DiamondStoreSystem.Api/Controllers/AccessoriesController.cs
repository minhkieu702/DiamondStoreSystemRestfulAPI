using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using DiamondStoreSystem.BusinessLayer.ResquestModels;
using DiamondStoreSystem.BusinessLayer.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondStoreSystem.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccessoriesController : ControllerBase
    {
        private readonly IAccessoryService _accessoryService;

        public AccessoriesController(IAccessoryService accessoryService)
        {
            _accessoryService = accessoryService;
        }

        [HttpGet("Enum")]
        public IActionResult GetEnum() => Ok(_accessoryService.GetEnum());

        [HttpPost("SearchByCategory")]
        public IActionResult GetByOrigin([FromBody] Dictionary<string, object> grades) => Ok(_accessoryService.GetByCategory(grades));

        [HttpGet("GetAllWithAllField")]
        public IActionResult GetAllWithAllField() => Ok(_accessoryService.GetAllWithAllField());

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_accessoryService.GetAll().Result);

        [HttpGet("GetByIdWithAllField/{id}")]
        public IActionResult GetByIdWithAllField(string id) => Ok(_accessoryService.IsExist(id).Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id) => Ok(_accessoryService.GetById(id).Result);

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AccessoryRequestModel model) => Ok(_accessoryService.Create(model).Result);

        [HttpPut("Update/{id}")]
        public IActionResult Update([FromBody] AccessoryRequestModel model, string id) => Ok(_accessoryService.Update(id, model).Result);
        
        [HttpPut("Unblock/{id}")]
        public IActionResult UnBlock(string id) => Ok(_accessoryService.UnBlock(id).Result);

        [HttpDelete("Block/{id}")]
        public IActionResult Block(string id) => Ok(_accessoryService.Block(id).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(string id) => Ok(_accessoryService.Delete(id).Result);
    }
}
