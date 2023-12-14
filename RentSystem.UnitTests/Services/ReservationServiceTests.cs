using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Enums;
using RentSystem.Core.Exceptions;
using RentSystem.Services.Services;

namespace RentSystem.UnitTests.Services
{
    internal class ReservationServiceTests
    {
        private Mock<IReservationRepository> _reservationRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IItemRepository> _itemRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IReservationService _reservationService;

        [SetUp]
        public void Setup()
        {
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _mapperMock = new Mock<IMapper>();
            _reservationService = new ReservationService(_reservationRepositoryMock.Object, _userRepositoryMock.Object, _itemRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ReservationsExist_MapsAndReturnsReservation()
        {
            var reservations = new List<Reservation>();
            for(int i = 0; i < 10; i++) 
                reservations.Add(GenerateReservation());

            _reservationRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(reservations);
            _mapperMock.Setup(m => m.Map<List<GetReservationDTO>>(reservations)).Returns(new List<GetReservationDTO>());

            var result = await _reservationService.GetAllAsync();

            Assert.That(result, Is.InstanceOf<List<GetReservationDTO>>());
            _reservationRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetReservationDTO>>(reservations), Times.Once);
        }

        [Test]
        public async Task GetAsync_ReservationExists_MapsAndReturnsReservation()
        {
            var reservation = GenerateReservation();

            _reservationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(reservation);
            _mapperMock.Setup(mapper => mapper.Map<GetReservationDTO>(It.IsAny<Reservation>())).Returns(new GetReservationDTO());

            var result = await _reservationService.GetAsync(reservation.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GetReservationDTO>());

            _reservationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetReservationDTO>(It.IsAny<Reservation>()), Times.Once);
        }

        [Test, AutoData]
        public async Task GetAsync_ReservationDoesNotExist_ThrowsException(int id)
        {
            _reservationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Reservation>());
            _mapperMock.Setup(mapper => mapper.Map<GetReservationDTO>(It.IsAny<Reservation>())).Returns(new GetReservationDTO());

            Assert.ThrowsAsync<NotFoundException>(async () => await _reservationService.GetAsync(id));

            _reservationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetReservationDTO>(It.IsAny<Reservation>()), Times.Never);
        }

        [Test, AutoData]
        public async Task CreateAsync_ReservationExists_ThrowsException(int userId)
        {
            ReservationDTO reservationDTO = GenerateReservationDTO();

            _reservationRepositoryMock.Setup(repo => repo.GetByTimeIntervalAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Reservation> {GenerateReservation()});
            _mapperMock.Setup(mapper => mapper.Map<Reservation>(It.IsAny<ReservationDTO>())).Returns(new Reservation { UserId = userId, User = new User()});

            Assert.ThrowsAsync<AlreadyExistsException>(async () => await _reservationService.CreateAsync(reservationDTO, userId));
            _reservationRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Reservation>()), Times.Never);
            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Never);
            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<Reservation>(It.IsAny<ReservationDTO>()), Times.Never);
            _reservationRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Reservation>()), Times.Never);
        }

        [Test, AutoData]
        public async Task CreateAsync_UserDoesNotExist_ThrowsException(int userId)
        {
            ReservationDTO reservationDTO = GenerateReservationDTO();

            _reservationRepositoryMock.Setup(repo => repo.GetByTimeIntervalAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Reservation> {});
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<User>());
            _mapperMock.Setup(mapper => mapper.Map<Reservation>(It.IsAny<ReservationDTO>())).Returns(new Reservation { UserId = userId, User = new User() });

            Assert.ThrowsAsync<NotFoundException>(async () => await _reservationService.CreateAsync(reservationDTO, userId));
            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<Reservation>(It.IsAny<ReservationDTO>()), Times.Never);
            _reservationRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Reservation>()), Times.Never);
        }

        [Test, AutoData]
        public async Task CreateAsync_ItemDoesNotExist_ThrowsException(int userId)
        {
            ReservationDTO reservationDTO = GenerateReservationDTO();

            _reservationRepositoryMock.Setup(repo => repo.GetByTimeIntervalAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Reservation> { });
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new User() { Id = userId});
            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Item>());
            _mapperMock.Setup(mapper => mapper.Map<Reservation>(It.IsAny<ReservationDTO>())).Returns(new Reservation { UserId = userId, User = new User() });

            Assert.ThrowsAsync<NotFoundException>(async () => await _reservationService.CreateAsync(reservationDTO, userId));
            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<Reservation>(It.IsAny<ReservationDTO>()), Times.Never);
            _reservationRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Reservation>()), Times.Never);
        }

        [Test, AutoData]
        public async Task CreateAsync_PassesChecks_CreatesAndReturnsMapped(int userId)
        {
            ReservationDTO reservationDTO = GenerateReservationDTO();

            _reservationRepositoryMock.Setup(repo => repo.GetByTimeIntervalAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Reservation> { });
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new User() { Id = userId });
            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Item() { Id = reservationDTO.ItemId});
            _mapperMock.Setup(mapper => mapper.Map<Reservation>(It.IsAny<ReservationDTO>())).Returns(new Reservation { UserId = userId, User = new User() });

            
            await _reservationService.CreateAsync(reservationDTO, userId);

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<Reservation>(It.IsAny<ReservationDTO>()), Times.Once);
            _reservationRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Reservation>()), Times.Once);
        }

        [Test, AutoData]
        public async Task UpdateAsync_ReservationExists_Updates(int id)
        {
            ReservationDTO reservationDTO = GenerateReservationDTO();

            _reservationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Reservation());

            await _reservationService.UpdateAsync(id, reservationDTO);

            _reservationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _reservationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Reservation>()), Times.Once);
        }

        [Test, AutoData]
        public async Task UpdateAsync_ReservationDoesNotExist_ThrowsException(int id)
        {
            ReservationDTO reservationDTO = GenerateReservationDTO();

            _reservationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Reservation>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _reservationService.UpdateAsync(id, reservationDTO));

            _reservationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _reservationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Reservation>()), Times.Never);
        }

        [Test, AutoData]
        public async Task DeleteAsync_ReservationExists_Deletes(int id)
        {
            _reservationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Reservation());

            await _reservationService.DeleteAsync(id);

            _reservationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _reservationRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Reservation>()), Times.Once);
        }

        [Test, AutoData]
        public async Task DeleteAsync_ReservationDoesNotExist_ThrowsException(int id)
        {
            _reservationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Reservation>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _reservationService.DeleteAsync(id));

            _reservationRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _reservationRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Reservation>()), Times.Never);
        }

        private static Reservation GenerateReservation()
        {
            return new Reservation
            {
                Id = Faker.RandomNumber.Next(),
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddDays(7),
                Price = Faker.RandomNumber.Next(),
                ItemId = Faker.RandomNumber.Next(),
                Item = new Item() { Id = Faker.RandomNumber.Next() },
                UserId = Faker.RandomNumber.Next(),
                User = new User() { Id = Faker.RandomNumber.Next()}
            };
        }

        private static ReservationDTO GenerateReservationDTO()
        {
            return new ReservationDTO
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddDays(7),
                Price = Faker.RandomNumber.Next(),
                ItemId = Faker.RandomNumber.Next(),
            };
        }
    }
}

