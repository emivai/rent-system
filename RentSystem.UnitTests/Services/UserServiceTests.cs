using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Enums;
using RentSystem.Core.Exceptions;
using RentSystem.Services.Services;
using System.Text;
using RentSystem.Core.Extensions;

namespace RentSystem.UnitTests.Services
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ITokenManager> _tokenManagerMock;
        private Mock<IMapper> _mapperMock;
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenManagerMock = new Mock<ITokenManager>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepositoryMock.Object, _tokenManagerMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllAsync_UsersExist_MapsAndReturnsUsers()
        {
            var users = new List<User>();

            for (int i = 0; i < 5; i++)
                users.Add(GenerateUser());

            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            _mapperMock.Setup(mapper => mapper.Map<List<UserDTO>>(users)).Returns(new List<UserDTO>());

            var result = await _userService.GetAllAsync();

            Assert.That(result, Is.InstanceOf<List<UserDTO>>());
            _userRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<UserDTO>>(users), Times.Once);
        }

        [Test]
        public async Task GetAsync_UserExists_MapsAndReturnsUser()
        {
            var user = GenerateUser();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>())).Returns(new UserDTO());

            var result = await _userService.GetAsync(user.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<UserDTO>());

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDTO>(It.IsAny<User>()), Times.Once);
        }

        [Test, AutoData]
        public async Task GetAsync_UserDoesNotExist_ThrowsException(int id)
        {
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((User)null);
            _mapperMock.Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>())).Returns(new UserDTO());

            Assert.ThrowsAsync<NotFoundException>(async () => await _userService.GetAsync(id));

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDTO>(It.IsAny<User>()), Times.Never);
        }

        [Test, AutoData]
        public async Task CreateAsync_UserExists_CreatesAndReturnsMapped(int userId)
        {
            var userDTO = GenerateRegisterUserDTO();
            _userRepositoryMock.Setup(repo => repo.FirstOrDefault(It.IsAny<Func<User, bool>>())).Returns(It.IsAny<User>());
            _mapperMock.Setup(mapper => mapper.Map<User>(userDTO)).Returns(new User { Id = userId });

            await _userService.CreateAsync(userDTO);

            _userRepositoryMock.Verify(repo => repo.FirstOrDefault(It.IsAny<Func<User, bool>>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<User>(userDTO), Times.Once);
        }

        [Test, AutoData]
        public void CreateAsync_UserDoesNotExist_ThrowsException(int userId)
        {
            var userDTO = GenerateRegisterUserDTO();
            _userRepositoryMock.Setup(repo => repo.FirstOrDefault(It.IsAny<Func<User, bool>>())).Returns(new User { Id = userId });

            Assert.ThrowsAsync<BadRequestException>(async () => await _userService.CreateAsync(userDTO));

            _userRepositoryMock.Verify(repo => repo.FirstOrDefault(It.IsAny<Func<User, bool>>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<User>(It.IsAny<RegisterUserDTO>()), Times.Never);
        }

        [Test]
        public async Task LoginAsync_UserExists_ReturnsSuccessfullLoginDTO()
        {
            var loginUserDTO = GenerateLoginUserDTO();

            var user = GenerateUser();

            _userRepositoryMock.Setup(repo => repo.FirstOrDefault(It.IsAny<Func<User, bool>>())).Returns(user);
            _tokenManagerMock.Setup(manager => manager.CreateAccessTokenAsync(user)).Returns(Faker.Lorem.GetFirstWord());

            var result = await _userService.LoginAsync(loginUserDTO);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<SuccessfullLoginDTO>());

            _userRepositoryMock.Verify(repo => repo.FirstOrDefault(It.IsAny<Func<User, bool>>()), Times.Once);
            _tokenManagerMock.Verify(manager => manager.CreateAccessTokenAsync(user), Times.Once);
        }

        [Test]
        public async Task LoginAsync_UserDoesNotExist_ThrowsException()
        {
            var loginUserDTO = GenerateLoginUserDTO();

            _userRepositoryMock.Setup(repo => repo.FirstOrDefault(It.IsAny<Func<User, bool>>())).Returns(It.IsAny<User>());

            Assert.ThrowsAsync<BadRequestException>(async () => await _userService.LoginAsync(loginUserDTO));

            _userRepositoryMock.Verify(repo => repo.FirstOrDefault(It.IsAny<Func<User, bool>>()), Times.Once);
            _tokenManagerMock.Verify(manager => manager.CreateAccessTokenAsync(It.IsAny<User>()), Times.Never);
        }

        [Test, AutoData]
        public async Task UpdateAsync_UserExists_Updates(int userId)
        {
            UpdateUserDTO updateUserDTO = GenerateUpdateUserDTO();
            var existingUser = new User { Id = userId };

            _userRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(existingUser);

            await _userService.UpdateAsync(userId, updateUserDTO);

            _userRepositoryMock.Verify(repo => repo.GetAsync(userId), Times.Once);
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(existingUser), Times.Once);
        }

        [Test, AutoData]
        public void UpdateAsync_UserDoesNotExist_ThrowsException(int userId)
        {
            UpdateUserDTO updateUserDTO = GenerateUpdateUserDTO();
            _userRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(It.IsAny<User>());

            Assert.ThrowsAsync<NotFoundException>(async () => await _userService.UpdateAsync(userId, updateUserDTO));
        }

        [Test, AutoData]
        public async Task DeleteAsync_UserExists_Deletes(int userId)
        {
            _userRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(new User());

            await _userService.DeleteAsync(userId);

            _userRepositoryMock.Verify(repo => repo.GetAsync(userId), Times.Once);
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<User>()), Times.Once);
        }

        [Test, AutoData]
        public void DeleteAsync_UserDoesNotExist_ThrowsException(int userId)
        {
            _userRepositoryMock.Setup(repo => repo.GetAsync(userId)).ReturnsAsync((User)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _userService.DeleteAsync(userId));

            _userRepositoryMock.Verify(repo => repo.GetAsync(userId), Times.Once);
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<User>()), Times.Never);
        }


        private static LoginUserDTO GenerateLoginUserDTO()
        {
            return new LoginUserDTO
            {
                Email = Faker.Internet.Email(),
                Password = Faker.Lorem.GetFirstWord()
            };
        }

        private static User GenerateUser()
        {
            var hashedPassword = Faker.Lorem.GetFirstWord().Hash();

            return new User
            {
                Id = Faker.RandomNumber.Next(),
                Role = Faker.Enum.Random<Role>(),
                Name = Faker.Name.First(),
                Surname = Faker.Name.Last(),
                Phone = Faker.Phone.Number(),
                Email = Faker.Internet.Email(),
                City = Faker.Address.City(),
                HouseNumber = Faker.RandomNumber.Next().ToString(),
                PostCode = Faker.Address.ZipCode(),
                Password = hashedPassword.Hash,
                Salt = hashedPassword.Salt
            };
        }

        private static UpdateUserDTO GenerateUpdateUserDTO()
        {
            return new UpdateUserDTO
            {
                Name = Faker.Name.First(),
                Surname = Faker.Name.Last(),
                Phone = Faker.Phone.Number(),
                Email = Faker.Internet.Email(),
                City = Faker.Address.City(),
                HouseNumber = Faker.RandomNumber.Next().ToString(),
                PostCode = Faker.Address.ZipCode()
            };
        }

        private static RegisterUserDTO GenerateRegisterUserDTO()
        {
            return new RegisterUserDTO
            {
                Role = Faker.Enum.Random<Role>(),
                Name = Faker.Name.First(),
                Surname = Faker.Name.Last(),
                Phone = Faker.Phone.Number(),
                Email = Faker.Internet.Email(),
                City = Faker.Address.City(),
                HouseNumber = Faker.RandomNumber.Next().ToString(),
                PostCode = Faker.Address.ZipCode(),
                Password = Faker.Lorem.GetFirstWord(),
                ConfirmPassword = Faker.Lorem.GetFirstWord() 
            };
        }

    }
}
