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
    internal class ItemServiceTests
    {
        private Mock<IAdvertRepository> _advertRepositoryMock;
        private Mock<IItemRepository> _itemRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IItemService _itemService;

        [SetUp]
        public void SetUp()
        {
            _advertRepositoryMock = new Mock<IAdvertRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _itemService = new ItemService(_itemRepositoryMock.Object, _advertRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ItemsExist_MapsAndReturnsItems()
        {
            var items = new List<Item> ();
            for (int i = 0; i < 10; i++)
                items.Add(GenerateItem());

            _itemRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);
            _mapperMock.Setup(mapper => mapper.Map<List<GetItemDTO>>(items)).Returns(new List<GetItemDTO>());

            var result = await _itemService.GetAllAsync();

            Assert.That(result, Is.InstanceOf<List<GetItemDTO>>());
            _itemRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<GetItemDTO>>(items), Times.Once);
        }

        [Test]
        public async Task GetAsync_ItemExists_MapsAndReturnsItem()
        {
            var expectedItem = GenerateItem();

            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);
            _mapperMock.Setup(mapper => mapper.Map<GetItemDTO>(It.IsAny<Item>())).Returns(new GetItemDTO());

            var result = await _itemService.GetAsync(expectedItem.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GetItemDTO>());

            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetItemDTO>(It.IsAny<Item>()), Times.Once);
        }

        [Test, AutoData]
        public async Task GetAsync_ItemDoesNotExist_ThrowsException(int id)
        {
            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Item>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _itemService.GetAsync(id));

            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetItemDTO>(It.IsAny<Item>()), Times.Never);
        }

        [Test, AutoData]
        public async Task CreateAsync_UserExists_CreatesAndReturnsMapped(int userId)
        {
            ItemDTO itemDto = GenerateItemDTO();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = userId });
            _mapperMock.Setup(mapper => mapper.Map<Item>(It.IsAny<ItemDTO>())).Returns(new Item { UserId = userId, User = new User() });

            await _itemService.CreateAsync(itemDto, userId);

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<Item>(It.IsAny<ItemDTO>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test, AutoData]
        public async Task CreateAsync_UserDoesNotExist_ThrowsException(int userId)
        {
            ItemDTO itemDto = GenerateItemDTO();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<User>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _itemService.CreateAsync(itemDto, userId));

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<Advert>(It.IsAny<ItemDTO>()), Times.Never);
            _itemRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Item>()), Times.Never);
        }

        [Test, AutoData]
        public async Task UpdateAsync_ItemAndAdvertExists_CreatesAndReturnsMapped(int id)
        {
            ItemDTO itemDto = GenerateItemDTO();

            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Item());
            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Advert());

            await _itemService.UpdateAsync(id, itemDto);

            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test, AutoData]
        public async Task UpdateAsync_ItemDoesNotExist_ThrowsException(int id)
        {
            ItemDTO itemDto = GenerateItemDTO();

            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Item>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _itemService.UpdateAsync(id, itemDto));

            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Never);
            _itemRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Item>()), Times.Never);
        }

        [Test, AutoData]
        public async Task UpdateAsync_AdvertDoesNotExist_ThrowsException(int id)
        {
            ItemDTO itemDto = GenerateItemDTO();

            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Item());
            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Advert>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _itemService.UpdateAsync(id, itemDto));

            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Item>()), Times.Never);
        }

        [Test, AutoData]
        public async Task DeleteAsync_ItemExists_Deletes(int id)
        {
            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Item());

            await _itemService.DeleteAsync(id);

            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test, AutoData]
        public async Task DeleteAsync_ItemDoesNotExist_ThrowsException(int id)
        {
            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Item>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _itemService.DeleteAsync(id));

            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Item>()), Times.Never);
        }

        private static Item GenerateItem()
        {
            return new Item
            {
                Id = Faker.RandomNumber.Next(),
                Category = Faker.Enum.Random<Category>(),
                Name = Faker.Lorem.GetFirstWord(),
                Price = Faker.RandomNumber.Next(),
                State = Faker.Enum.Random<State>(),
                AdvertId = Faker.RandomNumber.Next(),
                Advert = new Advert() { Id = Faker.RandomNumber.Next() },
                ReservationId = Faker.RandomNumber.Next(),
                Reservation = new Reservation() { Id = Faker.RandomNumber.Next() },
                ContractId = Faker.RandomNumber.Next(),
                Contract = new Contract() {  Id = Faker.RandomNumber.Next() },
                UserId = Faker.RandomNumber.Next(),
                User = new User() { Id = Faker.RandomNumber.Next() }
            };
        }

        private static ItemDTO GenerateItemDTO()
        {
            return new ItemDTO
            {
                Category = Faker.Enum.Random<Category>(),
                Name = Faker.Lorem.GetFirstWord(),
                Price = Faker.RandomNumber.Next(),
                State = Faker.Enum.Random<State>(),
                AdvertId = Faker.RandomNumber.Next(),
            };
        }
    }
}
