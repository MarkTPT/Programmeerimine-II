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
    public class SchedulesControllerTest
    {
        //private readonly ScheduleServiceStub _scheduleController;
        private readonly Mock<IScheduleService> _scheduleServiceMock;
        private readonly SchedulesController _scheduleController;

        public SchedulesControllerTest()
        {
            _scheduleServiceMock = new Mock<IScheduleService>();
            _scheduleController = new SchedulesController(_scheduleServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_list_of_schedules()
        {
            //Arrange
            var paged = GetPagedScheduleListModel();
            _scheduleServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                .ReturnsAsync(() => paged);

            //Act
            var result = await _scheduleController.Index(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<ScheduleListModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            //Arrange
            string[] defaultNames = new[] { null, "Index" };
            var paged = GetPagedScheduleListModel();
            _scheduleServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                              .ReturnsAsync(() => paged);

            //Act
            var result = await _scheduleController.Index(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultNames);

        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            //Arrange
            _scheduleServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _scheduleController.Index(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            //Arrange
            _scheduleServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _scheduleController.Details(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_schedule_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _scheduleServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _scheduleController.Details(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_schedule_is_found()
        {
            //Arrange
            var model = GetScheduleDetailModel();
            var defaultViewNames = new[] { null, "Details" };
            _scheduleServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => model);

            //Act
            var result = await _scheduleController.Details(model.ScheduleId) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.IsType<ScheduleDetailModel>(result.Model);
        }

        [Fact]
        public async Task Create_should_redirect_to_Index()
        {
            //Arrange
            var model = GetScheduleModel();
            var response = new OperationResponse();
            _scheduleServiceMock.Setup(serv => serv.Create(model)).ReturnsAsync(() => response);

            //Act
            var result = await _scheduleController.Create(model) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
                        
        }

        [Fact]
        public async Task Create_should_stay_on_form_when_model_is_invalid()
        {
            var model = GetScheduleModel();
            var defaultViewNames = new[] { null, "Create" };

            //Act
            _scheduleController.ModelState.AddModelError("key", "errorMessage");
            var result = await _scheduleController.Create(model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.False(_scheduleController.ModelState.IsValid);
            Assert.IsType<ScheduleCreateModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_return_not_found_if_schedule_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _scheduleServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _scheduleController.Edit(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_badresult_when_model_is_null()
        {
            // Arrange
            var schedule = (ScheduleEditModel)null;
            var scheduleId = 1;

            // Act
            var result = await _scheduleController.Edit(scheduleId, schedule) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_shold_throw_DbUpdateConcurrencyException()
        {
            //Arrange
            var model = GetScheduleEditModel();
            var schedule = GetSchedule();
            var exception = new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() });
            _scheduleServiceMock.Setup(serv => serv.Save(It.IsAny<ScheduleEditModel>()))
                              .Throws(exception);
            _scheduleServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>())).ReturnsAsync(() => schedule);


            //Act
            //var result = await _scheduleController.Edit(model.ScheduleId, model);

            //Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _scheduleController.Edit(model.ScheduleId, model));
            //Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_not_found_if_schedule_does_not_exist_on_DbUpdateConcurrencyException()
        {
            //Arrange
            var model = GetScheduleEditModel();
            var exception = new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() });
            _scheduleServiceMock.Setup(serv => serv.Save(It.IsAny<ScheduleEditModel>()))
                              .Throws(exception);
            _scheduleServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var result = await _scheduleController.Edit(model.ScheduleId, model) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_should_return_default_view()
        {
            //Arrange
            string[] defaultNames = new[] { null, "Create" };

            //Act
            var result = _scheduleController.Create() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultNames);

        }

        [Fact]
        public async Task Edit_Post_should_return_not_found_if_id_is_not_correct()
        {
            //Arrange
            var model = GetScheduleEditModel();

            //Act
            var result = await _scheduleController.Edit(1000) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_redirect_to_index_after_saving_model()
        {
            //Arrange
            var model = GetScheduleEditModel();
            var response = new OperationResponse();
            _scheduleServiceMock.Setup(serv => serv.Save(model)).ReturnsAsync(() => response);

            //Act
            var result = await _scheduleController.Edit(model.ScheduleId, model) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_return_correct_model_after_editing()
        {
            //Arrange
            var model = GetScheduleEditModel();
            var dict = new ModelStateDictionary();
            _scheduleServiceMock.Setup(serv => serv.Save(It.IsAny<ScheduleEditModel>()));

            //Act
            _scheduleController.ModelState.AddModelError("key", "errorMessage");
            var result = await _scheduleController.Edit(model.ScheduleId, model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<ScheduleEditModel>(result.Model);
        }
        [Fact]
        public async Task Edit_should_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _scheduleController.Edit(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_ids_does_not_match()
        {
            // Arrange
            var productIdReal = 1;
            var productIdDampered = 2;
            var model = GetScheduleEditModel();
            model.ScheduleId = productIdDampered;

            // Act
            var result = await _scheduleController.Edit(productIdReal, model) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task Edit_should_return_correct_model()
        {
            //Arrange
            var model = GetScheduleEditModel();
            _scheduleServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                  .ReturnsAsync(() => model);

            //Act
            var result =await _scheduleController.Edit(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<ScheduleEditModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_save_schedule_data()
        {
            // Arrange
            var model = GetScheduleEditModel();
            var response = new OperationResponse();
            _scheduleServiceMock.Setup(ps => ps.Save(model))
                               .ReturnsAsync(() => response)
                               .Verifiable();

            // Act
            var result = await _scheduleController.Edit(model.ScheduleId, model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _scheduleServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            //Arrange
            var schedule = GetSchedule();
            var response = new OperationResponse();

            _scheduleServiceMock.Setup(ps => ps.GetForDelete(schedule.ScheduleId))
                   .ReturnsAsync(() => schedule)
                   .Verifiable();
            _scheduleServiceMock.Setup(serv => serv.Delete(It.IsAny<int>()))
                               .ReturnsAsync(() => response);

            //Act
            var result = await _scheduleController.DeleteConfirmed(schedule.ScheduleId) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_schedule()
        {
            // Arrange
            var schedule = GetSchedule();
            var response = new OperationResponse();
            _scheduleServiceMock.Setup(ps => ps.GetForDelete(schedule.ScheduleId))
                               .ReturnsAsync(() => schedule)
                               .Verifiable();
            _scheduleServiceMock.Setup(ps => ps.Delete(schedule.ScheduleId))
                               .ReturnsAsync(() => response)
                               .Verifiable();

            // Act
            var result = await _scheduleController.DeleteConfirmed(schedule.ScheduleId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _scheduleServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_shoul_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _scheduleController.Delete(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_schedule_is_null()
        {
            //Arrange
            _scheduleServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _scheduleController.Delete(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_return_not_found_if_given_id_does_not_match_model_id()
        {
            //Arrange
            var model = GetScheduleEditModel();
            int passedId = 565643;

            //Act
            var result = await _scheduleController.Edit(passedId, model) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
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

        private ScheduleDetailModel GetScheduleDetailModel()
        {
            return new ScheduleDetailModel { ScheduleId = 1 };
        }

        private ScheduleCreateModel GetScheduleModel()
        {
            return new ScheduleCreateModel { ScheduleId = 1 };
        }

        private ScheduleEditModel GetScheduleEditModel()
        {
            return new ScheduleEditModel { ScheduleId = 1 };
        }

        private Schedule GetSchedule()
        {
            return new Schedule { ScheduleId = 1 };
        }
    }
}
