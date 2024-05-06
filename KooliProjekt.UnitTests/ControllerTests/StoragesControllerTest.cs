using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using KooliProjekt.Controllers;
using Moq;
using KooliProjekt.Services;
using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.AspNetCore.Http;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class StoragesControllerTest
    {
        //private readonly StorageServiceStub _storageController;
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly StoragesController _storageController;

        public StoragesControllerTest()
        {
            _storageServiceMock = new Mock<IStorageService>();
            _storageController = new StoragesController(_storageServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_list_of_storages()
        {
            //Arrange
            var paged = GetPagedStorageListModel();
            _storageServiceMock.Setup(serv => serv.StorageList(It.IsAny<int>()))
                .ReturnsAsync(() => paged);

            //Act
            var result = await _storageController.Index(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<StorageListModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            //Arrange
            string[] defaultNames = new[] { null, "Index" };
            var paged = GetPagedStorageListModel();
            _storageServiceMock.Setup(serv => serv.StorageList(It.IsAny<int>()))
                              .ReturnsAsync(() => paged);

            //Act
            var result = await _storageController.Index(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultNames);

        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            //Arrange
            _storageServiceMock.Setup(serv => serv.StorageList(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _storageController.Index(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            //Arrange
            _storageServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _storageController.Details(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_storage_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _storageServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _storageController.Details(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_storage_is_found()
        {
            //Arrange
            var model = GetStorageDetailModel();
            var defaultViewNames = new[] { null, "Details" };
            _storageServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => model);

            //Act
            var result = await _storageController.Details(model.StorageID) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.IsType<StorageDetailModel>(result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            //Arrange
            var storage = GetStorage();
            var response = new OperationResponse();

            _storageServiceMock.Setup(ps => ps.GetForDelete(storage.StorageID))
                   .ReturnsAsync(() => storage)
                   .Verifiable();
            _storageServiceMock.Setup(serv => serv.Delete(It.IsAny<int>()));

            //Act
            var result = await _storageController.DeleteConfirmed(storage.StorageID) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_shoul_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _storageController.Delete(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_storage_is_null()
        {
            //Arrange
            _storageServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _storageController.Delete(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        private PagedResult<StorageListModel> GetPagedStorageListModel()
        {
            return new PagedResult<StorageListModel>()
            {
                Results = new List<StorageListModel>()
                {
                    new StorageListModel{ StorageID = 1, Kood = "AAA01" },
                    new StorageListModel{ StorageID = 2, Kood = "AAA02" }
                },
                selectList = new List<SelectListItem> 
                { 
                    new SelectListItem { Text = "AAA01", Value = "1" },
                    new SelectListItem { Text = "AAA02", Value = "2" }
                },
                CurrentPage = 1,
                RowCount = 3,
                PageCount = 5,
                PageSize = 2
            };
        }

        private StorageDetailModel GetStorageDetailModel()
        {
            return new StorageDetailModel { ArtistId = 10, Name = "John Doe", StorageID = 1, Kood = "AAA01" };
        }

        private Storage GetStorage()
        {
            return new Storage { StorageID = 1, SongId = 1, Kood = "AAA01" };
        }
    }
}
