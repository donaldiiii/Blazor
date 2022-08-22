using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        public MoviesController(ApplicationDbContext context,
            IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }
        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var poster = Convert.FromBase64String(movie.Poster);
                movie.Poster = await fileStorageService.SaveFile(poster, ".jpg", "movies");
            }

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }
            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }


        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            const int limit = 6;

            var moviesInTheatre = await context.Movies
                    .Where(x => x.InTheaters).Take(limit)
                    .OrderByDescending(x => x.ReleaseDate)
                    .ToListAsync();

            var todaysDate = DateTime.Today;

            var upcomingReleases = await context.Movies
                    .Where(x => x.ReleaseDate > todaysDate)
                    .OrderBy(x => x.ReleaseDate).Take(limit)
                    .ToListAsync();
            return new IndexPageDTO()
            {
                UpcomingReleases = upcomingReleases,
                Intheaters = moviesInTheatre,
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailsMovieDTO>> GetById(int id)
        {
            var movie = await context.Movies.Where(x => x.Id == id)
                .Include(x=>x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x=>x.MoviesActors).ThenInclude(x => x.Person)
                .FirstOrDefaultAsync();

            if (movie == null) { return NotFound(); }

            movie.MoviesActors = movie.MoviesActors.OrderBy(x => x.Order).ToList();

            var model = new DetailsMovieDTO();
            model.Movie = movie;
            model.Genres = movie.MoviesGenres.Select(x => x.Genre).ToList();
            model.Actors = movie.MoviesActors.Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    Picture = x.Person.Picture,
                    Character = x.Character,
                    Id = x.PersonId

                }).ToList();

            return model;
        }
    }
}
