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
    public class DiamondsController : ControllerBase
    {
        private readonly IDiamondService _diamondService;

        public DiamondsController(IDiamondService diamondService)
        {
            _diamondService = diamondService;
        }

        [HttpGet("Enum")]
        public IActionResult GetEnum() => Ok(_diamondService.GetEnum());

        [HttpPost("SearchByCategory")]
        public IActionResult GetByOrigin([FromBody] Dictionary<string, object> grades) => Ok(_diamondService.GetByCategory(grades));

        [HttpGet("GetCertificate/{id}")]
        public IActionResult GetCertificate(string id) => Ok(_diamondService.GetCertificate(id).Result);

        [HttpGet("GetAllWithAllField")]
        public IActionResult GetAllWithAllField() => Ok(_diamondService.GetAllWithAllField());

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_diamondService.GetAll().Result);

        [HttpGet("GetByIdWithAllField/{id}")]
        public IActionResult GetByIdWithAllField(string id) => Ok(_diamondService.IsExist(id).Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id) => Ok(_diamondService.GetById(id).Result);

        [HttpPost("Create")]
        public IActionResult Create([FromBody] DiamondRequestModel model) => Ok(_diamondService.Create(model).Result);

        [HttpPut("Update/{id}")]
        public IActionResult Update([FromBody] DiamondRequestModel model, string id) => Ok(_diamondService.Update(id, model).Result);

        [HttpPut("Unblock/{id}")]
        public IActionResult UnBlock(string id) => Ok(_diamondService.UnBlock(id).Result);

        [HttpDelete("Block/{id}")]
        public IActionResult Block(string id) => Ok(_diamondService.Block(id).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(string id) => Ok(_diamondService.Delete(id).Result);
    }
}
