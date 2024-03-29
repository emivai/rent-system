﻿using RentSystem.Core.Contracts.Repository;
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
            var advert = GenerateAdvert();

            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(advert);
            _mapperMock.Setup(mapper => mapper.Map<GetAdvertDTO>(It.IsAny<Advert>())).Returns(new GetAdvertDTO());

            var result = await _advertService.GetAsync(advert.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GetAdvertDTO>());

            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetAdvertDTO>(It.IsAny<Advert>()), Times.Once);
        }

        [Test, AutoData]
        public async Task GetAsync_AdvertDoesNotExist_ThrowsException(int id)
        {
            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Advert>());
            _mapperMock.Setup(mapper => mapper.Map<GetAdvertDTO>(It.IsAny<Advert>())).Returns(new GetAdvertDTO());

            Assert.ThrowsAsync<NotFoundException>(async () => await _advertService.GetAsync(id));

            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetAdvertDTO>(It.IsAny<Advert>()), Times.Never);
        }

        [Test, AutoData]
        public async Task CreateAsync_UserExists_CreatesAndReturnsMapped(int userId)
        {
            AdvertDTO advertDTO = GenerateAdvertDTO();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = userId});
            _mapperMock.Setup(mapper => mapper.Map<Advert>(It.IsAny<AdvertDTO>())).Returns(new Advert { UserId = userId, User = new User()});

            await _advertService.CreateAsync(advertDTO, userId);

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Advert>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<Advert>(It.IsAny<AdvertDTO>()), Times.Once);
        }

        [Test, AutoData]
        public async Task CreateAsync_UserDoesNotExist_ThrowsException(int userId)
        {
            AdvertDTO advertDTO = GenerateAdvertDTO();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<User>());
            _mapperMock.Setup(mapper => mapper.Map<Advert>(It.IsAny<AdvertDTO>())).Returns(new Advert { UserId = userId, User = new User() });

            Assert.ThrowsAsync<NotFoundException>(async () => await _advertService.CreateAsync(advertDTO, userId));

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Advert>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<Advert>(It.IsAny<AdvertDTO>()), Times.Never);
        }

        [Test, AutoData]
        public async Task UpdateAsync_AdvertExists_Updates(int id)
        {
            AdvertDTO advertDTO = GenerateAdvertDTO();

            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Advert());

            await _advertService.UpdateAsync(id, advertDTO);

            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Advert>()), Times.Once);
        }

        [Test, AutoData]
        public async Task UpdateAsync_AdvertDoesNotExist_ThrowsException(int id)
        {
            AdvertDTO advertDTO = GenerateAdvertDTO();

            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Advert>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _advertService.UpdateAsync(id, advertDTO));

            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Advert>()), Times.Never);
        }

        [Test, AutoData]
        public async Task DeleteAsync_AdvertExists_Deletes(int id)
        {
            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Advert());

            await _advertService.DeleteAsync(id);

            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Advert>()), Times.Once);
        }

        [Test, AutoData]
        public async Task DeleteAsync_AdvertDoesNotExist_ThrowsException(int id)
        {
            _advertRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Advert>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _advertService.DeleteAsync(id));

            _advertRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _advertRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Advert>()), Times.Never);
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

        private static AdvertDTO GenerateAdvertDTO()
        {
            return new AdvertDTO
            {
                Title = Faker.Lorem.GetFirstWord(),
                Description = Faker.Lorem.Sentence(),
                ImageUrl = Faker.Internet.Url(),
                VideoUrl = Faker.Internet.Url(),
                DeliveryType = Faker.Enum.Random<DeliveryType>(),
                RentStart = DateTime.Now,
                RentEnd = DateTime.Now.AddDays(7)
            };
        }
    }
}
