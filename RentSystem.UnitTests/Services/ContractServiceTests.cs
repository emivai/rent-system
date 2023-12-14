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
            var contractDTO = GenerateContract();
            var renter = new User { Id = userId };

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(renter);
            _itemRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(new Item { User = renter });
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(renter);
            _mapperMock.Setup(mapper => mapper.Map<Contract>(contractDTO))
           .Returns(new Contract());
            await _contractService.CreateAsync(contractDTO, userId);

            _contractRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Contract>()), Times.Once);
        }

        [Test, AutoData]
        public async Task CreateAsync_InvalidRenter_ThrowsNotFoundException(int userId)
        {
            var contractDTO = GenerateContract();

            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            Assert.ThrowsAsync<NotFoundException>(() => _contractService.CreateAsync(contractDTO, userId));
        }

        [Test]
        public async Task GetAllAsync_ReturnsListOfContracts()
        {
            var contracts = new List<Contract> { new Contract(), new Contract() };

            _contractRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contracts);

            _mapperMock.Setup(mapper => mapper.Map<List<GetContractDTO>>(contracts))
                       .Returns(contracts.Select(c => new GetContractDTO {}).ToList());

            var result = await _contractService.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<GetContractDTO>>());
            Assert.That(contracts.Count, Is.EqualTo(result.Count));
        }

        [Test, AutoData]
        public async Task GetAsync_ExistingContractId_ReturnsGetContractDTO(int contractId)
        {
            var existingContract = new Contract {};
            _contractRepositoryMock.Setup(repo => repo.GetAsync(contractId)).ReturnsAsync(existingContract);
            _mapperMock.Setup(mapper => mapper.Map<GetContractDTO>(existingContract))
                       .Returns(new GetContractDTO { });

            var result = await _contractService.GetAsync(contractId);

            Assert.IsNotNull(result);
        }

        [Test, AutoData]
        public void GetAsync_NonExistingContractId_ThrowsNotFoundException(int nonExistingContractId)
        {
            _contractRepositoryMock.Setup(repo => repo.GetAsync(nonExistingContractId))
                .ReturnsAsync((Contract)null);

            Assert.ThrowsAsync<NotFoundException>(() => _contractService.GetAsync(nonExistingContractId));
        }

        private static ContractDTO GenerateContract()
        {
            return new ContractDTO
            {
                RentLenght = Faker.RandomNumber.Next(),
                Price = (Double)Faker.RandomNumber.Next(),
                ItemId = Faker.RandomNumber.Next()
            };
        }
    }
}
