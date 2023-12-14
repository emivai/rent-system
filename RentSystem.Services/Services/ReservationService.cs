using AutoMapper;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Exceptions;

namespace RentSystem.Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ReservationService(IReservationRepository reservationRepository, IUserRepository userRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetReservationDTO>> GetAllAsync()
        {
            var reservation = await _reservationRepository.GetAllAsync();

            return _mapper.Map<List<GetReservationDTO>>(reservation);
        }

        public async Task<GetReservationDTO?> GetAsync(int id)
        {
            var reservation = await _reservationRepository.GetAsync(id);

            if (reservation == null) throw new NotFoundException("Reservation", id);

            return _mapper.Map<GetReservationDTO>(reservation);
        }

        public async Task CreateAsync(ReservationDTO reservationDTO, int userId)
        {
            var reservations = await _reservationRepository.GetByTimeIntervalAsync(reservationDTO.DateFrom, reservationDTO.DateTo);

            if (reservations.Any()) throw new AlreadyExistsException("Reservation");

            var user = await _userRepository.GetAsync(userId);

            if (user == null) throw new NotFoundException("User", userId);

            var item = await _itemRepository.GetAsync(reservationDTO.ItemId);

            if (item == null) throw new NotFoundException("Item", reservationDTO.ItemId);

            var reservation = _mapper.Map<Reservation>(reservationDTO);

            reservation.UserId = userId;
            reservation.User = user;
            reservation.Item = item;

            await _reservationRepository.CreateAsync(reservation);
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await _reservationRepository.GetAsync(id);

            if (reservation == null) throw new NotFoundException("Reservation", id);

            await _reservationRepository.DeleteAsync(reservation);
        }

        public async Task UpdateAsync(int id, ReservationDTO reservationDTO)
        {
            var reservation = await _reservationRepository.GetAsync(id);

            if (reservation == null) throw new NotFoundException("Reservation", id);

            reservation.DateFrom = reservationDTO.DateFrom;
            reservation.DateTo = reservationDTO.DateTo;
            reservation.Price = reservationDTO.Price;
            reservation.ItemId = reservationDTO.ItemId;

            await _reservationRepository.UpdateAsync(reservation);
        }
    }
}
