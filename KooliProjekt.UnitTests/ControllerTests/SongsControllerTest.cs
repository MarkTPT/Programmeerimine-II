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
    public class SongsControllerTest
    {
        //private readonly SongServiceStub _songController;
        private readonly Mock<ISongService> _songServiceMock;
        private readonly SongsController _songController;

        public SongsControllerTest()
        {
            _songServiceMock = new Mock<ISongService>();
            _songController = new SongsController(_songServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_list_of_songs()
        {
            //Arrange
            var paged = GetPagedSongListModel();
            _songServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                .ReturnsAsync(() => paged);

            //Act
            var result = await _songController.Index(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<SongListModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            //Arrange
            string[] defaultNames = new[] { null, "Index" };
            var paged = GetPagedSongListModel();
            _songServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                              .ReturnsAsync(() => paged);

            //Act
            var result = await _songController.Index(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultNames);

        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            //Arrange
            _songServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songController.Index(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            //Arrange
            _songServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songController.Details(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_song_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _songServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songController.Details(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_song_is_found()
        {
            //Arrange
            var model = GetSongListModel();
            var defaultViewNames = new[] { null, "Details" };
            _songServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => model);

            //Act
            var result = await _songController.Details(model.SongId) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.IsType<SongListModel>(result.Model);
        }

        [Fact]
        public async Task Create_should_redirect_to_Index()
        {
            //Arrange
            var model = GetSongModel();
            var response = new OperationResponse();
            _songServiceMock.Setup(serv => serv.Create(model)).ReturnsAsync(() => response);

            //Act
            var result = await _songController.Create(model) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
                        
        }

        [Fact]
        public async Task Create_should_stay_on_form_when_model_is_invalid()
        {
            var model = GetSongModel();
            var defaultViewNames = new[] { null, "Create" };

            //Act
            _songController.ModelState.AddModelError("key", "errorMessage");
            var result = await _songController.Create(model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.False(_songController.ModelState.IsValid);
            Assert.IsType<SongCreationModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_return_not_found_if_song_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _songServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songController.Edit(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_badresult_when_model_is_null()
        {
            // Arrange
            var song = (SongEditModel)null;
            var songId = 1;

            // Act
            var result = await _songController.Edit(songId, song) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_shold_throw_DbUpdateConcurrencyException()
        {
            //Arrange
            var model = GetSongEditModel();
            var song = GetSong();
            var exception = new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() });
            _songServiceMock.Setup(serv => serv.Edit(It.IsAny<SongEditModel>()))
                              .Throws(exception);
            _songServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>())).ReturnsAsync(() => song);


            //Act
            //var result = await _songController.Edit(model.SongId, model);

            //Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _songController.Edit(model.SongId, model));
            //Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_not_found_if_song_does_not_exist_on_DbUpdateConcurrencyException()
        {
            //Arrange
            var model = GetSongEditModel();
            var exception = new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() });
            _songServiceMock.Setup(serv => serv.Edit(It.IsAny<SongEditModel>()))
                              .Throws(exception);
            _songServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var result = await _songController.Edit(model.SongId, model) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_Post_should_return_not_found_if_id_is_not_correct()
        {
            //Arrange
            var model = GetSongModel();

            //Act
            var result = await _songController.Edit(1000) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_redirect_to_index_after_saving_model()
        {
            //Arrange
            var model = GetSongEditModel();
            var response = new OperationResponse();
            _songServiceMock.Setup(serv => serv.Edit(model));

            //Act
            var result = await _songController.Edit(model.SongId, model) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_return_correct_model_after_editing()
        {
            //Arrange
            var model = GetSongEditModel();
            var dict = new ModelStateDictionary();
            _songServiceMock.Setup(serv => serv.Edit(It.IsAny<SongEditModel>()));

            //Act
            _songController.ModelState.AddModelError("key", "errorMessage");
            var result = await _songController.Edit(model.SongId, model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<SongEditModel>(result.Model);
        }
        [Fact]
        public async Task Edit_should_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _songController.Edit(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_ids_does_not_match()
        {
            // Arrange
            var productIdReal = 1;
            var productIdDampered = 2;
            var model = GetSongEditModel();
            model.SongId = productIdDampered;

            // Act
            var result = await _songController.Edit(productIdReal, model) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task Edit_should_return_correct_model()
        {
            //Arrange
            var model = GetSongEditModel();
            _songServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                  .ReturnsAsync(() => model);

            //Act
            var result =await _songController.Edit(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<SongEditModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_save_song_data()
        {
            // Arrange
            var model = GetSongEditModel();
            var response = new OperationResponse();
            _songServiceMock.Setup(ps => ps.Edit(model))
                               .Verifiable();

            // Act
            var result = await _songController.Edit(model.SongId, model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _songServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            //Arrange
            var song = GetSong();
            var response = new OperationResponse();

            _songServiceMock.Setup(ps => ps.GetForDelete(song.SongId))
                   .ReturnsAsync(() => song)
                   .Verifiable();
            _songServiceMock.Setup(serv => serv.Delete(It.IsAny<int>()));

            //Act
            var result = await _songController.DeleteConfirmed(song.SongId) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_shoul_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _songController.Delete(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_song_is_null()
        {
            //Arrange
            _songServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songController.Delete(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_return_not_found_if_given_id_does_not_match_model_id()
        {
            //Arrange
            var model = GetSongEditModel();
            int passedId = 565643;

            //Act
            var result = await _songController.Edit(passedId, model) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        private PagedResult<SongListModel> GetPagedSongListModel()
        {
            return new PagedResult<SongListModel>()
            {
                Results = new List<SongListModel>()
                {
                    new SongListModel{ SongId = 1, Title = "Title", ArtistId = 1 },
                    new SongListModel{ SongId = 2, Title = "Title2", ArtistId = 2 }
                },
                selectList = new List<SelectListItem> 
                { 
                    new SelectListItem { Text = "Title", Value = "1" },
                    new SelectListItem { Text = "Title2", Value = "2" }
                },
                CurrentPage = 1,
                RowCount = 3,
                PageCount = 5,
                PageSize = 2
            };
        }

        private SongListModel GetSongListModel()
        {
            return new SongListModel { SongId = 1, Title = "Title", ArtistId = 1 };
        }

        private SongCreationModel GetSongModel()
        {
            return new SongCreationModel { SongId = 1, Title = "Title", ArtistId = 1 };
        }

        private SongEditModel GetSongEditModel()
        {
            return new SongEditModel { SongId = 1, Title = "Title", ArtistId = 1 };
        }

        private Song GetSong()
        {
            return new Song { SongId = 1, Title = "Title", ArtistId = 1};
        }
    }
}
