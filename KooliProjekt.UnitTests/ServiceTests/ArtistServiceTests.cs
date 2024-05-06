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
    public class ArtistServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IArtistRepository> _artistRepositoryMock;
        private readonly Mock<IFileClient> _fileClientMock;
        private readonly Mock<IMapper> _objectMapperMock;

        private readonly ArtistService _artistService;

        public ArtistServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _fileClientMock = new Mock<IFileClient>();
            _objectMapperMock = new Mock<IMapper>();
            _artistRepositoryMock = new Mock<IArtistRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _uowMock.SetupGet(uow => uow.Artists)
                           .Returns(_artistRepositoryMock.Object);

            _artistService = new ArtistService(_uowMock.Object, _fileClientMock.Object, mapper);
        }

        [Fact]
        public async Task List_returns_list_of_artist_models()
        {
            // Arrange
            int page = 1;
            int id = 1;
            _artistRepositoryMock.Setup(pr => pr.Paged(page))
                                  .ReturnsAsync(() => new PagedResult<Artist> { Results = new List<Artist> { new Artist { ArtistId = 1 } } })
                                  .Verifiable();
            _artistRepositoryMock.Setup(ar => ar.GetSongTitles(id))
                                 .Returns(new List<string> { "abc", "def" });

            // Act
            var result = await _artistService.List(page);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.NotNull(result.Results.FirstOrDefault().SongTitles);
            Assert.IsType<PagedResult<ArtistSongListModel>>(result);
            _artistRepositoryMock.VerifyAll();
        }



        [Fact]
        public async Task GetForDetail_should_return_null_if_artist_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullArtist = (Artist)null;
            _artistRepositoryMock.Setup(pr => pr.Get(nonExistentId))
                                  .ReturnsAsync(() => nullArtist)
                                  .Verifiable();

            // Act
            var result = await _artistService.GetForDetail(nonExistentId);

            // Assert
            Assert.Null(result);
            _artistRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForDetail_should_return_artist()
        {
            // Arrange
            var id = 1;
            var artist = new Artist { ArtistId = id };
            _artistRepositoryMock.Setup(pr => pr.Get(id))
                                  .ReturnsAsync(() => artist)
                                  .Verifiable();

            // Act
            var result = await _artistService.GetForDetail(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArtistDetailModel>(result);
            _artistRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_null_if_artist_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullArtist = (Artist)null;
            _artistRepositoryMock.Setup(pr => pr.Get(nonExistentId))
                                  .ReturnsAsync(() => nullArtist)
                                  .Verifiable();

            // Act
            var result = await _artistService.GetForEdit(nonExistentId);

            // Assert
            Assert.Null(result);
            _artistRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_artist()
        {
            // Arrange
            var id = 1;
            var artist = new Artist { ArtistId = id };
            _artistRepositoryMock.Setup(pr => pr.Get(id))
                                  .ReturnsAsync(() => artist)
                                  .Verifiable();

            // Act
            var result = await _artistService.GetForEdit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArtistModel>(result);
            _artistRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_survive_null_model()
        {
            // Arrange
            var model = (ArtistModel)null;

            // Act
            var response = await _artistService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_artist()
        {
            // Arrange
            var id = 1;
            var model = new ArtistModel { ArtistId= id };
            var nullArtist = (Artist)null;
            _artistRepositoryMock.Setup(ar => ar.Get(id))
                                  .ReturnsAsync(() => nullArtist)
                                  .Verifiable();

            // Act
            var response = await _artistService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_save_valid_product()
        {
            // Arrange
            var id = 1;
            var artist = new Artist { ArtistId = id };
            var artistModel = new ArtistModel { ArtistId = id, Name = "John Doe" };

            _artistRepositoryMock.Setup(pr => pr.Get(id))
                                  .ReturnsAsync(() => artist)
                                  .Verifiable();
            _uowMock.Setup(uow => uow.CompleteAsync())
                                     .Verifiable();

            // Act
            var response = await _artistService.Save(artistModel);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _artistRepositoryMock.VerifyAll();
            _uowMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_survive_null_model()
        {
            // Arrange
            int? nullId = null;

            // Act
            var response = await _artistService.Delete(nullId);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_handles_null_product()
        {
            // Arrange
            var id = 1;
            var artistToDelete = (Artist)null;

            _artistRepositoryMock.Setup(ar => ar.Get(id))
                                  .ReturnsAsync(() => artistToDelete)
                                  .Verifiable();

            // Act
            var response = await _artistService.Delete(id);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _artistRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_deletes_product()
        {
            // Arrange
            var id = 1;
            var productToDelete = new Artist { ArtistId = id };

            _artistRepositoryMock.Setup(ar => ar.Get(id))
                                  .ReturnsAsync(() => productToDelete)
                                  .Verifiable();
            _artistRepositoryMock.Setup(ar => ar.Delete(id))
                                  .Verifiable();
            _uowMock.Setup(uow => uow.CompleteAsync())
                           .Verifiable();

            // Act
            var response = await _artistService.Delete(id);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _artistRepositoryMock.VerifyAll();
            _uowMock.VerifyAll();
        }

        [Fact]
        public async Task SaveFile_should_save_file()
        {
            // Arrange
            var id = "1";
            int page = 1;
            var files = new List<IFormFile>
            {
                new FakeFormFile { FileName = "test.txt" }
            };
            _fileClientMock.Setup(fc => fc.Save(ContainerNames.Artists,
                                                It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()
                                                ))
                           .Verifiable();

            // Act
            var result = await _artistService.SaveFile(files, id, page);

            // Assert
            Assert.NotNull(result);
            _fileClientMock.VerifyAll();
        }

        [Fact]
        public async Task SaveFile_should_save_file_if_id_is_null()
        {
            // Arrange
            string id = null;
            int page = 1;
            var files = new List<IFormFile>
            {
                new FakeFormFile { FileName = "test.txt" }
            };
            _fileClientMock.Setup(fc => fc.Save(ContainerNames.Artists,
                                                It.IsAny<string>(), It.IsAny<Stream>()
                                                ))
                           .Verifiable();

            // Act
            var result = await _artistService.SaveFile(files, id, page);

            // Assert
            Assert.NotNull(result);
            _fileClientMock.VerifyAll();
        }


        [Fact]
        public async Task SaveFile_should_fill_model()
        {
            // Arrange
            string stringId = "1";
            int id = 1;
            int page = 1;
            var files = new List<IFormFile>
            {
                new FakeFormFile { FileName = "test.txt" }
            };

            _artistRepositoryMock.Setup(pr => pr.Paged(page))
                                  .ReturnsAsync(() => new PagedResult<Artist> { Results = new List<Artist> { new Artist { ArtistId = 1 } }  })
                                  .Verifiable();
            _fileClientMock.Setup(fc => fc.Save(ContainerNames.Artists,
                                                It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()
                                           ))
                           .Verifiable();
            _artistRepositoryMock.Setup(ar => ar.GetSongTitles(id)).Returns(() => new List<string> { "abc", "def" })
                                 .Verifiable();
            _artistRepositoryMock.Setup(ar => ar.GetArtists()).ReturnsAsync(() => new List<SelectListItem> { new SelectListItem                                                   { Text = "cba", Value = "1"} })
                                 .Verifiable();

            // Act
            var result = await _artistService.SaveFile(files, stringId, page);

            // Assert
            Assert.NotNull(result.Results);
            Assert.NotNull(result.Results.FirstOrDefault().SongTitles);
            Assert.NotNull(result.selectList);
            _fileClientMock.VerifyAll();
        }

        [Fact]
        public async Task About_returns_list_of_artist_and_files()
        {
            // Arrange
            string container = ContainerNames.Artists;
            _artistRepositoryMock.Setup(ar => ar.All())
                                  .ReturnsAsync(() => new List<Artist>())
                                  .Verifiable();
            _fileClientMock.Setup(ar => ar.List(container))
                                 .ReturnsAsync(() => new List<string>())
                                 .Verifiable();

            // Act
            var result = await _artistService.GetForAbout();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Files);
            Assert.NotNull(result.Artists);
            Assert.IsType<ArtistAboutModel>(result);
            _artistRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteFile_should_delete_file()
        {
            // Arrange
            var container = ContainerNames.Artists;
            var filename = "test.txt";
            _fileClientMock.Setup(fc => fc.Delete(container, filename))
                           .Verifiable();

            // Act
            var result = await _artistService.DeleteFile(container, filename);

            // Assert
            Assert.NotNull(result);
            _fileClientMock.VerifyAll();
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
    }
}
