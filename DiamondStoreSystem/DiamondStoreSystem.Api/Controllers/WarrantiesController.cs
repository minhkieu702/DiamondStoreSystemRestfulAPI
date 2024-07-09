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
    public class WarrantiesController : ControllerBase
    {
        private readonly IWarrantyService _warrantyService;

        public WarrantiesController(IWarrantyService warrantyService)
        {
            _warrantyService = warrantyService;
        }

        [HttpGet("Enum")]
        public IActionResult GetEnum() => Ok(_warrantyService.GetEnum());

        [HttpGet("GetAllWithAllField")]
        public IActionResult GetAllWithAllField() => Ok(_warrantyService.GetAllWithAllField());

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_warrantyService.GetAll().Result);

        [HttpGet("GetByIdWithAllField/{id}")]
        public IActionResult GetByIdWithAllField(string id) => Ok(_warrantyService.IsExist(id).Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id) => Ok(_warrantyService.GetById(id).Result);

        [HttpPost("Create")]
        public IActionResult Create([FromBody] WarrantyRequestModel model) => Ok(_warrantyService.Create(model).Result);

        [HttpPut("Update/{id}")]
        public IActionResult Update([FromBody] WarrantyRequestModel model, string id) => Ok(_warrantyService.Update(id, model).Result);

        [HttpPut("Unblock/{id}")]
        public IActionResult UnBlock(string id) => Ok(_warrantyService.UnBlock(id).Result);

        [HttpDelete("Block/{id}")]
        public IActionResult Block(string id) => Ok(_warrantyService.Block(id).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(string id) => Ok(_warrantyService.Delete(id, "WarrantyID").Result);
    }
}
