using AutoMapper;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Exceptions;

namespace RentSystem.Services.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RequestService(IRequestRepository requestRepository, IUserRepository userRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetRequestDTO>> GetAllAsync()
        {
            var requests = await _requestRepository.GetAllAsync();

            return _mapper.Map<List<GetRequestDTO>>(requests);
        }

        public async Task<GetRequestDTO> GetAsync(int id)
        {
            var request = await _requestRepository.GetAsync(id);

            return _mapper.Map<GetRequestDTO>(request);
        }

        public async Task CreateAsync(RequestDTO requestDTO, int userId)
        {
            var user = await _userRepository.GetAsync(userId) ?? throw new NotFoundException("User was not found");

            var request = _mapper.Map<Request>(requestDTO);

            request.IsAvailable = false;
            request.UserId = userId;
            request.User = user;

            await _requestRepository.CreateAsync(request);
        }

        public async Task UpdateAsync(int id, RequestDTO requestDTO)
        {
            var request = await _requestRepository.GetAsync(id) ?? throw new NotFoundException("Request", id);

            request.Category = requestDTO.Category;
            request.Count = requestDTO.Count;

            await _requestRepository.UpdateAsync(request);
        }

        public async Task DeleteAsync(int id)
        {
            var request = await _requestRepository.GetAsync(id);

            if (request == null) throw new NotFoundException("Request", id);

            await _requestRepository.DeleteAsync(request);
        }
    }
}
