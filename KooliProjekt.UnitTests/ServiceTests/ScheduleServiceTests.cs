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
    public class ScheduleServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IScheduleRepository> _scheduleRepositoryMock;
        private readonly Mock<IMapper> _objectMapperMock;

        private readonly ScheduleService _scheduleService;

        public ScheduleServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _objectMapperMock = new Mock<IMapper>();
            _scheduleRepositoryMock = new Mock<IScheduleRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _uowMock.SetupGet(uow => uow.Schedule)
                           .Returns(_scheduleRepositoryMock.Object);

            _scheduleService = new ScheduleService(_uowMock.Object, mapper);
        }

        [Fact]
        public async Task GetForEdit_should_return_null_if_schedule_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullSchedule = (Schedule)null;
            _scheduleRepositoryMock.Setup(pr => pr.Get(nonExistentId))
                                  .ReturnsAsync(() => nullSchedule)
                                  .Verifiable();

            // Act
            var result = await _scheduleService.GetForEdit(nonExistentId);

            // Assert
            Assert.Null(result);
            _scheduleRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_schedule()
        {
            // Arrange
            var id = 1;
            var schedule = new Schedule { ScheduleId = id };
            _scheduleRepositoryMock.Setup(pr => pr.Get(id))
                                  .ReturnsAsync(() => schedule)
                                  .Verifiable();

            // Act
            var result = await _scheduleService.GetForEdit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ScheduleEditModel>(result);
            _scheduleRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_survive_null_model()
        {
            // Arrange
            var model = (ScheduleEditModel)null;

            // Act
            var response = await _scheduleService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_schedule()
        {
            // Arrange
            var id = 1;
            var model = new ScheduleEditModel { ScheduleId= id };
            var nullSchedule = (Schedule)null;
            _scheduleRepositoryMock.Setup(ar => ar.Get(id))
                                  .ReturnsAsync(() => nullSchedule)
                                  .Verifiable();

            // Act
            var response = await _scheduleService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_should_survive_null_model()
        {
            // Arrange
            int? nullId = null;

            // Act
            var response = await _scheduleService.Delete(nullId);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_handles_null_product()
        {
            // Arrange
            var id = 1;
            var scheduleToDelete = (Schedule)null;

            _scheduleRepositoryMock.Setup(ar => ar.Get(id))
                                  .ReturnsAsync(() => scheduleToDelete)
                                  .Verifiable();

            // Act
            var response = await _scheduleService.Delete(id);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _scheduleRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_deletes_product()
        {
            // Arrange
            var id = 1;
            var productToDelete = new Schedule { ScheduleId = id };

            _scheduleRepositoryMock.Setup(ar => ar.Get(id))
                                  .ReturnsAsync(() => productToDelete)
                                  .Verifiable();
            _scheduleRepositoryMock.Setup(ar => ar.Delete(id))
                                  .Verifiable();
            _uowMock.Setup(uow => uow.CompleteAsync())
                           .Verifiable();

            // Act
            var response = await _scheduleService.Delete(id);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _scheduleRepositoryMock.VerifyAll();
            _uowMock.VerifyAll();
        }

        private PagedResult<ScheduleListModel> GetPagedScheduleListModel()
        {
            return new PagedResult<ScheduleListModel>()
            {
                Results = new List<ScheduleListModel>()
                {
                    new ScheduleListModel{ ScheduleId = 1 },
                    new ScheduleListModel{ ScheduleId = 2 }
                },
                selectList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Schedule 1", Value = "1" },
                    new SelectListItem { Text = "Schedule 2", Value = "2" }
                },
                CurrentPage = 1,
                RowCount = 3,
                PageCount = 5,
                PageSize = 2
            };
        }
    }
}
