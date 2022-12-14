using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class PeopleController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;

        private readonly IMapper mapper;

        public PeopleController(ApplicationDbContext context,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromQuery]PaginationDTO pagination)
        {
            var queryable = context.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, pagination.RecordsPerPage);
            var listOfPersons = await queryable.Paginate(pagination).ToListAsync();
            return listOfPersons;
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Person>(); }
            return await context.People.Where(x => x.Name.Contains(searchText))
                .Take(5)
                .ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                person.Picture = await fileStorageService.SaveFile(personPicture, ".jpg", "people");
            }
            context.Add(person);
            await context.SaveChangesAsync();
            return person.Id;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) return NotFound();
            return person;
        }
        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            var foundPerson = await context.People.FirstOrDefaultAsync(x => x.Id == person.Id);
            if (foundPerson == null) return NotFound();
            foundPerson = mapper.Map(person, foundPerson);
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                foundPerson.Picture = await fileStorageService.EditFile(
                    Convert.FromBase64String(person.Picture),
                    "jpg", "people", foundPerson.Picture);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var people = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (people == null) return NotFound();
            context.Remove(people);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
