using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Enums;
using RentSystem.Core.Exceptions;
using RentSystem.Services.Services;

namespace RentSystem.UnitTests.Services
{
    public class AdvertServiceTests
    {
        private Mock<IAdvertRepository> _advertRepositoryMock;
        private Mock<IItemRepository> _itemRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IAdvertService _advertService;

        [SetUp]
        public void SetUp()
        {
            _advertRepositoryMock = new Mock<IAdvertRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _advertService = new AdvertService(_advertRepositoryMock.Object, _itemRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }

        [Test, AutoData]
        public async Task GetAllAsync_AdvertsExist_MapsAndReturnsAdverts(Category category)
        {
            var adverts = new List<Advert>();

            for(int i = 0; i < 10; i++) 
                adverts.Add(GenerateAdvert());

            _advertRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Category>())).ReturnsAsync(adverts);
            _mapperMock.Setup(mapper => mapper.Map<List<GetAdvertDTO>>(adverts)).Returns(new List<GetAdvertDTO>());

            var result = await _advertService.GetAllAsync(category);

            Assert.That(result, Is.InstanceOf<List<GetAdvertDTO>>());
            _advertRepositoryMock.Verify(repo => repo.GetAllAsync(category), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<GetAdvertDTO>>(adverts), Times.Once);

        }

        [Test]
        public async Task GetAsync_AdvertExists_MapsAndReturnsAdvert()
        {
            var expectedAdvert = GenerateAdvert();

            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(expectedAdvert);
            _mapperMock.Setup(mapper => mapper.Map<GetAdvertDTO>(It.IsAny<Advert>())).Returns(new GetAdvertDTO());

            var result = await _advertService.GetAsync(expectedAdvert.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GetAdvertDTO>());

            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetAdvertDTO>(It.IsAny<Advert>()), Times.Once);
        }

        [Test, AutoData]
        public async Task GetAsync_AdvertDoesNotExist_ThrowsException(int id)
        {
            Advert? advert = null;

            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(advert);
            _mapperMock.Setup(mapper => mapper.Map<GetAdvertDTO>(It.IsAny<Advert>())).Returns(new GetAdvertDTO());

            Assert.ThrowsAsync<NotFoundException>(async () => await _advertService.GetAsync(id));

            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetAdvertDTO>(It.IsAny<Advert>()), Times.Never);
        }

        private static Advert GenerateAdvert()
        {
            return new Advert
            {
                Id = Faker.RandomNumber.Next(),
                Title = Faker.Lorem.GetFirstWord(),
                Description = Faker.Lorem.Sentence(),
                ImageUrl = Faker.Internet.Url(),
                VideoUrl = Faker.Internet.Url(),
                DeliveryType = Faker.Enum.Random<DeliveryType>(),
                RentStart = DateTime.Now,
                RentEnd = DateTime.Now.AddDays(7),
                Items = new List<Item> { new Item { Id = Faker.RandomNumber.Next()}, new Item { Id = Faker.RandomNumber.Next() } },
                UserId = Faker.RandomNumber.Next(),
                User = new User() { Id = Faker.RandomNumber.Next() }
            };
        }
    }
}
