using Moq;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Enums;
using RentSystem.Core.Exceptions;
using RentSystem.Services.Services;

namespace RentSystem.UnitTests.Services
{
    internal class RequestServiceTests
    {
        private Mock<IRequestRepository> _requestRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IRequestService _requestService;

        [SetUp]
        public void SetUp()
        {
            _requestRepositoryMock = new Mock<IRequestRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _requestService = new RequestService(_requestRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllAsync_RequestsExist_MapsAndReturnsRequests()
        {
            var requests = new List<Request>();
            for (int i = 0; i < 10; i++)
                requests.Add(GenerateRequest());

            _requestRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(requests);
            _mapperMock.Setup(mapper => mapper.Map<List<GetRequestDTO>>(requests)).Returns(new List<GetRequestDTO>());

            var result = await _requestService.GetAllAsync();

            Assert.That(result, Is.InstanceOf<List<GetRequestDTO>>());
            _requestRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<GetRequestDTO>>(requests), Times.Once);
        }

        [Test]
        public async Task GetAsync_RequestExists_MapsAndReturnsRequest()
        {
            var expectedRequest = GenerateRequest();

            _requestRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(expectedRequest);
            _mapperMock.Setup(mapper => mapper.Map<GetRequestDTO>(It.IsAny<Request>())).Returns(new GetRequestDTO());

            var result = await _requestService.GetAsync(expectedRequest.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GetRequestDTO>());

            _requestRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetRequestDTO>(It.IsAny<Request>()), Times.Once);
        }

        [Test, AutoData]
        public async Task GetAsync_RequestDoesNotExist_ThrowsException(int id)
        {
            _requestRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Request>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _requestService.GetAsync(id));

            _requestRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetRequestDTO>(It.IsAny<Request>()), Times.Never);
        }

        [Test, AutoData]
        public async Task CreateAsync_UserExists_CreatesAndReturnsMapped(int userId)
        {
            RequestDTO requestDTO = GenerateRequestDTO();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new User());
            _mapperMock.Setup(mapper => mapper.Map<Request>(It.IsAny<RequestDTO>())).Returns(new Request());

            await _requestService.CreateAsync(requestDTO, userId);

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _requestRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Request>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<Request>(It.IsAny<RequestDTO>()), Times.Once);
        }

        [Test, AutoData]
        public async Task CreateAsync_UserDoesNotExist_CreatesAndReturnsMapped(int userId, bool isAvailable)
        {
            RequestDTO requestDTO = GenerateRequestDTO();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<User>());
            
            Assert.ThrowsAsync<NotFoundException>(async () => await _requestService.CreateAsync(requestDTO, userId));
            
            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _requestRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Request>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<Request>(It.IsAny<RequestDTO>()), Times.Never);
        }

        [Test, AutoData]
        public async Task UpdateAsync_RequestExists_Updates(int id)
        {
            RequestDTO requestDTO = GenerateRequestDTO();

            _requestRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Request());

            await _requestService.UpdateAsync(id, requestDTO);

            _requestRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _requestRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Request>()), Times.Once);
        }

        [Test, AutoData]
        public async Task UpdateAsync_RequestDoesNotExist_Updates(int id)
        {
            RequestDTO requestDTO = GenerateRequestDTO();

            _requestRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Request>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _requestService.UpdateAsync(id, requestDTO));

            _requestRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _requestRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Request>()), Times.Never);
        }

        [Test, AutoData]
        public async Task DeleteAsync_RequestExists_Deletes(int id)
        {
            _requestRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Request());

            await _requestService.DeleteAsync(id);

            _requestRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _requestRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Request>()), Times.Once);
        }

        [Test, AutoData]
        public async Task DeleteAsync_RequestDoesNotExist_ThrowsException(int id)
        {
            _requestRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Request>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _requestService.DeleteAsync(id));

            _requestRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _requestRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Request>()), Times.Never);
        }

        private static Request GenerateRequest()
        {
            return new Request
            {
                Id = Faker.RandomNumber.Next(),
                Category = Faker.Enum.Random<Category>(),
                Count = Faker.RandomNumber.Next(),
                IsAvailable = Faker.Boolean.Random(),
                UserId = Faker.RandomNumber.Next(),
                User = new User() { Id = Faker.RandomNumber.Next() }
            };
        }

        private static RequestDTO GenerateRequestDTO()
        {
            return new RequestDTO
            {
                Category = Faker.Enum.Random<Category>(),
                Count = Faker.RandomNumber.Next()
            };
        }
    }
}
