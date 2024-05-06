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
    public class HomeControllerTest
    {
        private readonly Mock<IScheduleService> _scheduleServiceMock;
        private readonly HomeController _homeController;

        public HomeControllerTest()
        {
            _scheduleServiceMock = new Mock<IScheduleService>();
            _homeController = new HomeController(_scheduleServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_model()
        {
            //Arrange
            var model = getHomeListModel();
            _scheduleServiceMock.Setup(serv => serv.GetForHome())
                                .ReturnsAsync(() => model);

            //Act
            var result = await _homeController.Index() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<List<HomeListModel>>(result.Model);
        }

        [Fact]
        public async Task Index_should_return_not_found_if_model_is_null()
        {
            //Arrange
            _scheduleServiceMock.Setup(serv => serv.GetForHome())
                                .ReturnsAsync(() => null);

            //Act
            var result = await _homeController.Index() as NotFoundResult;

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task Privacy_should_return_view()
        {
            //Act
            var result = _homeController.Privacy() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        private List<HomeListModel> getHomeListModel()
        {
            return new List<HomeListModel>
            {
                new HomeListModel{ Date = DateTime.Now, ScheduleId = 1, Songs = new List<SongSchedule>()},
                new HomeListModel{ Date = DateTime.Now.AddMinutes(10), ScheduleId = 2, Songs = new List<SongSchedule>()}
            };
        }

        
    }
}
