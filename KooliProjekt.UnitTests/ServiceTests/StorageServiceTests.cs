using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.FileAccess;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class StorageServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IStorageRepository> _storageRepositoryMock;
        private readonly Mock<IMapper> _objectMapperMock;

        private readonly StorageService _storageService;

        public StorageServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _objectMapperMock = new Mock<IMapper>();
            _storageRepositoryMock = new Mock<IStorageRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _uowMock.SetupGet(uow => uow.Storage)
                           .Returns(_storageRepositoryMock.Object);

            _storageService = new StorageService(_uowMock.Object, mapper);
        }

        [Fact]
        public async Task List_returns_list_of_storage_models()
        {
            // Arrange
            int page = 1;
            _storageRepositoryMock.Setup(pr => pr.Paged(page))
                                  .ReturnsAsync(() => new PagedResult<Storage> { Results = new List<Storage> { new Storage { StorageID = 1 } } })
                                  .Verifiable();

            // Act
            var result = await _storageService.StorageList(page);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.IsType<PagedResult<StorageListModel>>(result);
            _storageRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForDetail_should_return_null_if_storage_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullStorage = (Storage)null;
            _storageRepositoryMock.Setup(pr => pr.Get(nonExistentId))
                                  .ReturnsAsync(() => nullStorage)
                                  .Verifiable();

            // Act
            var result = await _storageService.GetForDetail(nonExistentId);

            // Assert
            Assert.Null(result);
            _storageRepositoryMock.VerifyAll();
        }
    }
}
