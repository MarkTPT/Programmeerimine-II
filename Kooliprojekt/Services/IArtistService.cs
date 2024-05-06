using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IArtistService
    {
        Task<ArtistModel> GetForEdit(int id);
        Task<PagedResult<ArtistSongListModel>> List(int page);
        Task<PagedResult<ArtistSongListModel>> SaveFile(List<IFormFile> formFiles, string artistId, int page);
        Task<ArtistAboutModel> DeleteFile(string container, string fileName);
        Task<ArtistDetailModel> GetForDetail(int id);
        //Task<OperationResponse> Create(ArtistModel model);
        Task<OperationResponse> Save(ArtistModel model);
        Task<Artist> GetForDelete(int id);
        Task<OperationResponse> Delete(int? id);
        Task<ArtistAboutModel> GetForAbout();
        Task<PagedResult<ArtistDto>> ApiGetList();
        Task<PagedResult<string>> GetArtistNames(int page);
        void ChangeEntityStateTo(ArtistModel artist, EntityState state);
    }
}
