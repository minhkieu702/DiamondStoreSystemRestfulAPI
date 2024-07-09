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
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("Enum")]
        public IActionResult GetEnum() => Ok(_accountService.GetEnum());

        [HttpGet("GetAllWithAllField")]
        public IActionResult GetAllWithAllField() => Ok(_accountService.GetAllWithAllField());

        [HttpGet("GetAll")]
        public IActionResult GetAll() => Ok(_accountService.GetAll().Result);

        [HttpGet("GetByIdWithAllField/{id}")]
        public IActionResult GetByIdWithAllField(string id) => Ok(_accountService.IsExist(id).Result);

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id) => Ok(_accountService.GetById(id).Result);

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AccountRequestModel model) => Ok(_accountService.Create(model).Result);

        [HttpPut("Update/{id}")]
        public IActionResult Update([FromBody] AccountRequestModel model, string id) => Ok(_accountService.Update(id, model).Result);

        [HttpPut("Unblock/{id}")]
        public IActionResult UnBlock(string id) => Ok(_accountService.UnBlock(id).Result);
        [HttpDelete("Block/{id}")]
        public IActionResult Block(string id) => Ok(_accountService.Block(id).Result);
        [HttpPost("ChangeWorkingSchedule/{id}")]
        public IActionResult Block(string id, int workingSchedule) => Ok(_accountService.ChangeWorkingSchedule(id, workingSchedule).Result);

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(string id) => Ok(_accountService.Delete(id).Result);
    }
}
