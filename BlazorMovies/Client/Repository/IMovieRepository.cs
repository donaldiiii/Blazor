using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IMovieRepository
    {
        Task<int> CreateMovie(Movie movie);
        Task DeleteMovie(int id);
        Task<DetailsMovieDTO> GetDetailsMovieDTO(int id);
        Task<IndexPageDTO> GetIndexPageDTO();
        Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO);
    }
}