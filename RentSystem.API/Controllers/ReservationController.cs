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
    [Route("api/reservations")]
    public class ReservationController : BaseController
    {
        private readonly IReservationService _reservationService;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAuthorizationService _authorizationService;

        public ReservationController(IReservationService reservationService, IReservationRepository reservationRepository, IAuthorizationService authorizationService)
        {
            _reservationService = reservationService;
            _reservationRepository = reservationRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _reservationService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _reservationService.GetAsync(id));
        }

        [HttpPost]
        [AuthorizeRole(Role.Renter)]
        public async Task<IActionResult> Create(ReservationDTO reservationDTO)
        {
            var userId = GetUserId();

            await _reservationService.CreateAsync(reservationDTO, userId);

            return Ok();
        }

        [HttpPut("{id}")]
        [AuthorizeRole(Role.Renter)]
        public async Task<IActionResult> Update(ReservationDTO reservationDTO, int id)
        {
            var reservation = await _reservationRepository.GetAsync(id);

            var reservationAuthResult = await _authorizationService.AuthorizeAsync(User, reservation, PolicyNames.SameUser);

            if (!reservationAuthResult.Succeeded)
            {
                return Forbid();
            }

            await _reservationService.UpdateAsync(id, reservationDTO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [AuthorizeRole(Role.Renter)]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationRepository.GetAsync(id);

            var reservationAuthResult = await _authorizationService.AuthorizeAsync(User, reservation, PolicyNames.SameUser);

            if (!reservationAuthResult.Succeeded)
            {
                return Forbid();
            }

            await _reservationService.DeleteAsync(id);

            return NoContent();
        }
    }
}
