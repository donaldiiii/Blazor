using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IGenreRepository
    {
        Task CreateGenre(Genre genre);
        Task DeleteGenre(int id);
        Task<List<Genre>> GetGenre();
        Task<Genre> GetGenre(int Id);
        Task UpdateGenre(Genre genre);
    }
}