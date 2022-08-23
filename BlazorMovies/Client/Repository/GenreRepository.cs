using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private string url = "api/genres";
        private readonly IHttpService httpService;
        public GenreRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }
        public async Task CreateGenre(Genre genre)
        {
            var response = await httpService.Post<Genre, int>(url, genre);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task<List<Genre>> GetGenre()
        {
            var response = await httpService.Get<List<Genre>>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task<Genre> GetGenre(int Id)
        {
            var response = await httpService.Get<Genre>($"{url}/{Id}");
            await response.ThrowIfNotSuccessfulResponse();
            return response.Response;
        }
        public async Task UpdateGenre(Genre genre)
        {
            var response = await httpService.Put(url, genre);
            await response.ThrowIfNotSuccessfulResponse();
        }  
        public async Task DeleteGenre(int id)
        {
            var response = await httpService.Delete($"{url}/{id}");
            await response.ThrowIfNotSuccessfulResponse();
        }
    }
}
