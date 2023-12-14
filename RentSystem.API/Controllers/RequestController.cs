using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentSystem.API.Extensions;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Enums;
using RentSystem.Core.Policies;

namespace RentSystem.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestController : BaseController
    {
        private readonly IRequestService _requestService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IRequestRepository _requestRepository;

        public RequestController(IRequestService requestService, IUserService userService, IAuthorizationService authorizationService, IRequestRepository requestRepository) 
        {
            _requestService = requestService;
            _userService = userService;
            _authorizationService = authorizationService;
            _requestRepository = requestRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _requestService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _requestService.GetAsync(id));
        }

        [HttpPost]
        [AuthorizeRole(Role.Renter)]
        public async Task<IActionResult> Create(RequestDTO requestDTO)
        {
            var userId = GetUserId();

            await _requestService.CreateAsync(requestDTO, userId);
  
            return Ok();
        }

        [HttpPut("{id}")]
        [AuthorizeRole(Role.Renter)]
        public async Task<IActionResult> Update(int id, RequestDTO requestDTO)
        {
            var request = await _requestRepository.GetAsync(id);

            var requestAuthResult = await _authorizationService.AuthorizeAsync(User, request, PolicyNames.SameUser);

            if (!requestAuthResult.Succeeded)
            {
                return Forbid();
            }

            await _requestService.UpdateAsync(id, requestDTO);
            return NoContent();

        }

        [HttpDelete]
        [AuthorizeRole(Role.Renter)]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _requestRepository.GetAsync(id);

            var requestAuthResult = await _authorizationService.AuthorizeAsync(User, request, PolicyNames.SameUser);

            if (!requestAuthResult.Succeeded)
            {
                return Forbid();
            }

            await _requestService.DeleteAsync(id);
            return NoContent();
        }
    }
}
