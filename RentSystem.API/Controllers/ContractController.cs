using Microsoft.AspNetCore.Mvc;
using RentSystem.API.Extensions;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Enums;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    public class ContractController : BaseController
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _contractService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _contractService.GetAsync(id));
        }

        [HttpPost]
        [AuthorizeRole(Role.Renter)]
        public async Task<IActionResult> Create(ContractDTO contractDTO)
        {
            var userId = GetUserId();

            await _contractService.CreateAsync(contractDTO, userId);

            return Ok();
        }
    }
}
