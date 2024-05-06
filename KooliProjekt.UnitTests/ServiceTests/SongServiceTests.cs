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
    public class SongServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ISongRepository> _songRepositoryMock;
        private readonly Mock<IMapper> _objectMapperMock;

        private readonly SongService _songService;

        public SongServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _objectMapperMock = new Mock<IMapper>();
            _songRepositoryMock = new Mock<ISongRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _uowMock.SetupGet(uow => uow.Songs)
                           .Returns(_songRepositoryMock.Object);

            _songService = new SongService(_uowMock.Object, mapper);
        }

        [Fact]
        public async Task GetForDetail_should_return_null_if_song_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullSong = (Song)null;
            _songRepositoryMock.Setup(pr => pr.Get(nonExistentId))
                                  .ReturnsAsync(() => nullSong)
                                  .Verifiable();

            // Act
            var result = await _songService.GetForDetail(nonExistentId);

            // Assert
            Assert.Null(result);
            _songRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_null_if_song_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullSong = (Song)null;
            _songRepositoryMock.Setup(pr => pr.Get(nonExistentId))
                                  .ReturnsAsync(() => nullSong)
                                  .Verifiable();

            // Act
            var result = await _songService.GetForEdit(nonExistentId);

            // Assert
            Assert.Null(result);
            _songRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_survive_null_model()
        {
            // Arrange
            var model = (SongCreationModel)null;

            // Act
            var response = await _songService.Create(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_song()
        {
            // Arrange
            var id = 1;
            var model = new SongCreationModel { SongId= id };
            var nullSong = (Song)null;
            _songRepositoryMock.Setup(ar => ar.Get(id))
                                  .ReturnsAsync(() => nullSong)
                                  .Verifiable();

            // Act
            var response = await _songService.Create(model);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
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
    }
}
