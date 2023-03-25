using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _itemService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _itemService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemDTO itemDTO) 
        {
            await _itemService.CreateAsync(itemDTO);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ItemDTO itemDTO)
        {
            await _itemService.UpdateAsync(id, itemDTO);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemService.DeleteAsync(id);

            return Ok();
        }
    }
}