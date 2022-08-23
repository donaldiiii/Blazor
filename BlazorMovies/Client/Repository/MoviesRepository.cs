using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string url = "/api/movies";
        private readonly IHttpService httpService;
        public MovieRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<IndexPageDTO> GetIndexPageDTO()
        {
            return await httpService.GetHelper<IndexPageDTO>(url);
        }
        public async Task<DetailsMovieDTO> GetDetailsMovieDTO(int id)
        {
            return await httpService.GetHelper<DetailsMovieDTO>($"{url}/{id}");
        }
        public async Task<int> CreateMovie(Movie movie)
        {
            movie.Id = 0;
            var response = await httpService.Post<Movie, int>(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task DeleteMovie(int id)
        {
            var response = await httpService.Delete($"{url}/{id}");
            await response.ThrowIfNotSuccessfulResponse();
        }
    }
}
