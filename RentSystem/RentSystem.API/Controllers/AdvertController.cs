using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/adverts")]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        public AdvertController(IAdvertService advertService)
        {
            _advertService = advertService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _advertService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _advertService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvertDTO advertDTO)
        {
            await _advertService.CreateAsync(advertDTO);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AdvertDTO advertDTO)
        {
            await _advertService.UpdateAsync(id, advertDTO);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _advertService.DeleteAsync(id);

            return Ok();
        }
    }
}
