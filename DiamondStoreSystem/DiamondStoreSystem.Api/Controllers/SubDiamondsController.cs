using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResquestModels;

namespace SubDiamondStoreSystem.API.Controllers
{
    [EnableCors("AllowAnyOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubDiamondsController : ControllerBase
    {
        private readonly ISubDiamondService _subDiamondService;

        public SubDiamondsController(ISubDiamondService subDiamondService)
        {
            _subDiamondService = subDiamondService;
        }

        [HttpGet("Enum")]
        public IActionResult GetEnum() => Ok(_subDiamondService.GetEnum());

        [HttpGet("GetAllWithAllField")]
        public IActionResult GetAllWithAllField() => Ok(_subDiamondService.GetAllWithAllField());

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_subDiamondService.GetAll().Result);

        [HttpGet("GetByIdWithAllField/{id}")]
        public IActionResult GetByIdWithAllField(string id) => Ok(_subDiamondService.IsExist(id).Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id) => Ok(_subDiamondService.GetById(id).Result);

        [HttpPost("Create")]
        public IActionResult Create([FromBody] SubDiamondRequestModel model) => Ok(_subDiamondService.Create(model).Result);

        [HttpPut("Update/{id}")]
        public IActionResult Update([FromBody] SubDiamondRequestModel model, string id) => Ok(_subDiamondService.Update(id, model).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(string id) => Ok(_subDiamondService.Delete(id, "SubDiamondID").Result);
    }
}
