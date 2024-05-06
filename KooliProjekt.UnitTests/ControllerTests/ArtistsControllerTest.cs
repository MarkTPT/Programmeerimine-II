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
    public class ArtistsControllerTest
    {
        //private readonly ArtistServiceStub _artistController;
        private readonly Mock<IArtistService> _artistServiceMock;
        private readonly ArtistsController _artistController;

        public ArtistsControllerTest()
        {
            _artistServiceMock = new Mock<IArtistService>();
            _artistController = new ArtistsController(_artistServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_list_of_artists()
        {
            //Arrange
            var paged = GetPagedArtistSongListModel();
            _artistServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                .ReturnsAsync(() => paged);

            //Act
            var result = await _artistController.Index() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<ArtistSongListModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            //Arrange
            string[] defaultNames = new[] { null, "Index" };
            var paged = GetPagedArtistSongListModel();
            _artistServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                              .ReturnsAsync(() => paged);

            //Act
            var result = await _artistController.Index() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultNames);

        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            //Arrange
            _artistServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _artistController.Index() as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task IndexHttpPost_should_return_not_found_if_model_is_null()
        {
            //Arrange
            _artistServiceMock.Setup(serv => serv.SaveFile(It.IsAny<List<IFormFile>>(), It.IsAny<string>(), It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _artistController.Index(new List<IFormFile>(), "1", 1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task IndexHttpPost_should_return_correct_model()
        {
            //Arrange
            var model = GetPagedArtistSongListModel();
            _artistServiceMock.Setup(serv => serv.SaveFile(It.IsAny<List<IFormFile>>(), It.IsAny<string>(), It.IsAny<int>()))
                              .ReturnsAsync(() => model);

            //Act
            var result = await _artistController.Index(new List<IFormFile>(), "1", 1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<ArtistSongListModel>);
        }

        [Fact]
        public async Task IndexHttpPost_should_return_badrequest_if_formFiles_is_null()
        {
            //Arrange
            var model = GetPagedArtistSongListModel();
            var formFiles = (List<IFormFile>)null;
            string artistId = "1";
            int page = 1;

            //Act
            var result = await _artistController.Index(formFiles, artistId, page) as BadRequestResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            //Arrange
            _artistServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _artistController.Details(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_artist_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _artistServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _artistController.Details(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_artist_is_found()
        {
            //Arrange
            var model = GetArtistDetailModel();
            var defaultViewNames = new[] { null, "Details" };
            _artistServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => model);

            //Act
            var result = await _artistController.Details(model.ArtistId) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.IsType<ArtistDetailModel>(result.Model);
        }

        [Fact]
        public async Task About_returns_correct_result()
        {
            //Arrange
            var defaultViewNames = new[] { null, "About" };
            var model = GetArtistAboutModel();
            _artistServiceMock.Setup(serv => serv.GetForAbout())
                              .ReturnsAsync(() => model);

            //Act
            var result = await _artistController.About() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.IsType<ArtistAboutModel>(result.Model);
        }

        [Fact]
        public async Task About_should_return_not_found_if_model_is_null()
        {
            //Arrange
            _artistServiceMock.Setup(serv => serv.GetForAbout())
                              .ReturnsAsync(() => null);

            //Act
            var result = await _artistController.About() as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AboutHttpPost_should_return_correct_model()
        {
            //Arrange
            string container = "container";
            string fileName = "fileName";
            var model = GetArtistAboutModel();
            _artistServiceMock.Setup(serv => serv.DeleteFile(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(() => model);

            //Act
            var result = await _artistController.About(container, fileName) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<ArtistAboutModel>(result.Model);
        }

        [Fact]
        public async Task AboutHttpPost_should_return_not_found_if_model_is_null()
        {
            //Arrange
            string container = "container";
            string fileName = "fileName";
            _artistServiceMock.Setup(serv => serv.DeleteFile(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _artistController.About(container, fileName) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_should_redirect_to_Index()
        {
            //Arrange
            var model = GetArtistModel();
            var response = new OperationResponse();
            _artistServiceMock.Setup(serv => serv.Save(model)).ReturnsAsync(() => response);

            //Act
            var result = await _artistController.Create(model) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
                        
        }

        [Fact]
        public async Task Create_should_stay_on_form_when_model_is_invalid()
        {
            var model = GetArtistModel();
            var defaultViewNames = new[] { null, "Create" };

            //Act
            _artistController.ModelState.AddModelError("key", "errorMessage");
            var result = await _artistController.Create(model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.False(_artistController.ModelState.IsValid);
            Assert.IsType<ArtistModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_return_not_found_if_artist_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _artistServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _artistController.Edit(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_badresult_when_model_is_null()
        {
            // Arrange
            var artist = (ArtistModel)null;
            var artistId = 1;

            // Act
            var result = await _artistController.Edit(artistId, artist) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_shold_throw_DbUpdateConcurrencyException()
        {
            //Arrange
            var model = GetArtistModel();
            var artist = GetArtist();
            var exception = new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() });
            _artistServiceMock.Setup(serv => serv.Save(It.IsAny<ArtistModel>()))
                              .Throws(exception);
            _artistServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>())).ReturnsAsync(() => artist);


            //Act
            //var result = await _artistController.Edit(model.ArtistId, model);

            //Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _artistController.Edit(model.ArtistId, model));
            //Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_not_found_if_artist_does_not_exist_on_DbUpdateConcurrencyException()
        {
            //Arrange
            var model = GetArtistModel();
            var exception = new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() });
            _artistServiceMock.Setup(serv => serv.Save(It.IsAny<ArtistModel>()))
                              .Throws(exception);
            _artistServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var result = await _artistController.Edit(model.ArtistId, model) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_should_return_default_view()
        {
            //Arrange
            string[] defaultNames = new[] { null, "Create" };

            //Act
            var result = _artistController.Create() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultNames);

        }

        [Fact]
        public async Task Edit_Post_should_return_not_found_if_id_is_not_correct()
        {
            //Arrange
            var model = GetArtistModel();

            //Act
            var result = await _artistController.Edit(1000) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_redirect_to_index_after_saving_model()
        {
            //Arrange
            var model = GetArtistModel();
            var response = new OperationResponse();
            _artistServiceMock.Setup(serv => serv.Save(model)).ReturnsAsync(() => response);

            //Act
            var result = await _artistController.Edit(model.ArtistId, model) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_return_correct_model_after_editing()
        {
            //Arrange
            var model = GetArtistModel();
            var dict = new ModelStateDictionary();
            _artistServiceMock.Setup(serv => serv.Save(It.IsAny<ArtistModel>()));

            //Act
            _artistController.ModelState.AddModelError("key", "errorMessage");
            var result = await _artistController.Edit(model.ArtistId, model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<ArtistModel>(result.Model);
        }
        [Fact]
        public async Task Edit_should_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _artistController.Edit(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_ids_does_not_match()
        {
            // Arrange
            var productIdReal = 1;
            var productIdDampered = 2;
            var model = GetArtistModel();
            model.ArtistId = productIdDampered;

            // Act
            var result = await _artistController.Edit(productIdReal, model) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task Edit_should_return_correct_model()
        {
            //Arrange
            var model = GetArtistModel();
            _artistServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                  .ReturnsAsync(() => model);

            //Act
            var result =await _artistController.Edit(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<ArtistModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_save_artist_data()
        {
            // Arrange
            var model = GetArtistModel();
            var response = new OperationResponse();
            _artistServiceMock.Setup(ps => ps.Save(model))
                               .ReturnsAsync(() => response)
                               .Verifiable();

            // Act
            var result = await _artistController.Edit(model.ArtistId, model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _artistServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            //Arrange
            var artist = GetArtist();
            var response = new OperationResponse();

            _artistServiceMock.Setup(ps => ps.GetForDelete(artist.ArtistId))
                   .ReturnsAsync(() => artist)
                   .Verifiable();
            _artistServiceMock.Setup(serv => serv.Delete(It.IsAny<int>()))
                               .ReturnsAsync(() => response);

            //Act
            var result = await _artistController.DeleteConfirmed(artist.ArtistId) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_artist()
        {
            // Arrange
            var artist = GetArtist();
            var response = new OperationResponse();
            _artistServiceMock.Setup(ps => ps.GetForDelete(artist.ArtistId))
                               .ReturnsAsync(() => artist)
                               .Verifiable();
            _artistServiceMock.Setup(ps => ps.Delete(artist.ArtistId))
                               .ReturnsAsync(() => response)
                               .Verifiable();

            // Act
            var result = await _artistController.DeleteConfirmed(artist.ArtistId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            _artistServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_shoul_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _artistController.Delete(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_artist_is_null()
        {
            //Arrange
            _artistServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _artistController.Delete(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_correct_model()
        {
            //Arrange
            var model = new Artist
            {
                ArtistId = 1,
                Name = "John Doe",
                Description = "lorem ipsum",
                Songs = new List<Song>
                {
                    new Song
                    {
                        SongId = 1, ArtistId = 1, Title = "abc", Tempo = 2
                    },
                    new Song
                    {
                        SongId = 2, ArtistId = 1, Title = "cba", Tempo = 1
                    }
                }
            };
            _artistServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(model);

            //Act
            var result = await _artistController.Delete(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<Artist>(result.Model);
        }

        [Fact]
        public async Task Edit_return_not_found_if_given_id_does_not_match_model_id()
        {
            //Arrange
            var model = GetArtistModel();
            int passedId = 565643;

            //Act
            var result = await _artistController.Edit(passedId, model) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        private PagedResult<ArtistSongListModel> GetPagedArtistSongListModel()
        {
            return new PagedResult<ArtistSongListModel>()
            {
                Results = new List<ArtistSongListModel>()
                {
                    new ArtistSongListModel{ ArtistId = 13, Name = "John Doe"},
                    new ArtistSongListModel{ ArtistId = 22, Name = "Tom Oak"}
                },
                selectList = new List<SelectListItem> 
                { 
                    new SelectListItem { Text = "John Doe", Value = "1" },
                    new SelectListItem { Text = "Tom Oak", Value = "2" }
                },
                CurrentPage = 1,
                RowCount = 3,
                PageCount = 5,
                PageSize = 2
            };
        }

        private ArtistDetailModel GetArtistDetailModel()
        {
            return new ArtistDetailModel { ArtistId = 10, Name = "John Doe", Description = "lorem ipsum" };
        }

        private ArtistAboutModel GetArtistAboutModel()
        {
            return new ArtistAboutModel
            {
                Artists = new List<Artist>
                {
                    new Artist{ ArtistId = 13, Name = "John Doe", Description = "lorem ipsum"},
                    new Artist{ ArtistId = 27, Name = "Tom Oak", Description = "lorem ipsum"}
                },
                Files = new List<string> { "John_Wick.jpg", "HarryPotter.jfif"}
            };
        }

        private ArtistModel GetArtistModel()
        {
            return new ArtistModel { ArtistId = 19, Name = "John Doe", Description = "lorem ipsum." };
        }

        private Artist GetArtist()
        {
            return new Artist { ArtistId = 19, Name = "John Doe", Description = "lorem ipsum", Songs = new List<Song>() };
        }
    }
}
