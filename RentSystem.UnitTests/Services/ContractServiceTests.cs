using NUnit.Framework.Constraints;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Exceptions;
using RentSystem.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentSystem.UnitTests.Services
{
    internal class ContractServiceTests
    {
        private IContractService _contractService;
        private Mock<IContractRepository> _contractRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IItemRepository> _itemRepositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _contractRepositoryMock = new Mock<IContractRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _mapperMock = new Mock<IMapper>();

            _contractService = new ContractService(
                _contractRepositoryMock.Object,
                _userRepositoryMock.Object,
                _itemRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Test, AutoData]
        public async Task CreateAsync_ValidInput_CreatesContract(int userId)
        {
            var contractDTO = GenerateContractDTO();
            var renter = new User { Id = userId };

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(renter);
            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Item { User = renter, Id = contractDTO.ItemId });
            _mapperMock.Setup(mapper => mapper.Map<Contract>(contractDTO))
           .Returns(new Contract());
            await _contractService.CreateAsync(contractDTO, userId);

            _contractRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Contract>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Exactly(2));
            _mapperMock.Verify(mapper => mapper.Map<Contract>(It.IsAny<ContractDTO>()), Times.Once);
        }

        [Test, AutoData]
        public async Task CreateAsync_InvalidRenter_ThrowsNotFoundException(int userId)
        {
            var contractDTO = GenerateContractDTO();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<User>);

            Assert.ThrowsAsync<NotFoundException>(() => _contractService.CreateAsync(contractDTO, userId));

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _contractRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Contract>()), Times.Never);
            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map<Contract>(It.IsAny<ContractDTO>()), Times.Never);
        }
        [Test, AutoData]
        public async Task CreateAsync_InvalidItem_ThrowsNotFoundException(int userId)
        {
            var contractDTO = GenerateContractDTO();
            var renter = new User { Id = userId };

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(renter);
            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<Item>());

            Assert.ThrowsAsync<NotFoundException>(() => _contractService.CreateAsync(contractDTO, userId));

            _userRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<Contract>(It.IsAny<ContractDTO>()), Times.Never);
            _contractRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Contract>()), Times.Never);

        }

        [Test]
        public async Task GetAllAsync_ReturnsListOfContracts()
        {
            var contracts = new List<Contract> { GenerateContract(), GenerateContract() };

            _contractRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contracts);

            _mapperMock.Setup(mapper => mapper.Map<List<GetContractDTO>>(contracts))
                       .Returns(contracts.Select(c => new GetContractDTO {}).ToList());

            var result = await _contractService.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<GetContractDTO>>());
            Assert.That(contracts.Count, Is.EqualTo(result.Count));

            _contractRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<GetContractDTO>>(It.IsAny<ICollection<Contract>>()), Times.Once);
        }

        [Test, AutoData]
        public async Task GetAsync_ExistingContractId_ReturnsGetContractDTO(int contractId)
        {
            var existingContract = GenerateContract();
            _contractRepositoryMock.Setup(repo => repo.GetAsync(contractId)).ReturnsAsync(existingContract);
            _mapperMock.Setup(mapper => mapper.Map<GetContractDTO>(existingContract))
                       .Returns(new GetContractDTO { });

            var result = await _contractService.GetAsync(contractId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GetContractDTO>());

            _contractRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetContractDTO>(It.IsAny<Contract>()), Times.Once);
        }

        [Test, AutoData]
        public void GetAsync_NonExistingContractId_ThrowsNotFoundException(int nonExistingContractId)
        {
            _contractRepositoryMock.Setup(repo => repo.GetAsync(nonExistingContractId))
                .ReturnsAsync(It.IsAny<Contract>);

            Assert.ThrowsAsync<NotFoundException>(() => _contractService.GetAsync(nonExistingContractId));

            _contractRepositoryMock.Verify(repo => repo.GetAsync(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetContractDTO>(It.IsAny<Contract>()), Times.Never);
        }

        private static ContractDTO GenerateContractDTO()
        {
            return new ContractDTO
            {
                RentLenght = Faker.RandomNumber.Next(),
                Price = (Double)Faker.RandomNumber.Next(),
                ItemId = Faker.RandomNumber.Next()
            };
        }

        private static Contract GenerateContract()
        {
            return new Contract
            {
                Id = Faker.RandomNumber.Next(),
                RentLength = Faker.RandomNumber.Next(),
                Price = (Double)Faker.RandomNumber.Next(),
                ItemId = Faker.RandomNumber.Next(),
                Item = new Item { Id = Faker.RandomNumber.Next() },
                RenterId = Faker.RandomNumber.Next(),
                Renter = new User { Id = Faker.RandomNumber.Next() },
                OwnerId = Faker.RandomNumber.Next(),
                Owner = new User { Id = Faker.RandomNumber.Next() }
            };
        }
    }
}
