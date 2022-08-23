using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTO;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private string url = "api/people";
        private readonly IHttpService httpService;
        public PersonRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<PaginatedResponse<List<Person>>> GetPerson(PaginationDTO paginationDTO)
        {
            return await httpService.GetHelper<List<Person>>(url, paginationDTO);
           
        }
        public async Task CreatePerson(Person person)
        {
            var response = await httpService.Post<Person, int>(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task<List<Person>> GetPeopleByName(string name)
        {
            var response = await httpService.Get<List<Person>>(url + "/search/" + name);
            await response.ThrowIfNotSuccessfulResponse();
            return response.Response;
        }

        public async Task UpdatePerson(Person person)
        {
            var response = await httpService.Put(url, person);
            await response.ThrowIfNotSuccessfulResponse();
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await httpService.GetHelper<Person>($"{url}/{id}");
        }
        public async Task DeletePeople(int id)
        {
            var response = await httpService.Delete($"{url}/{id}");
            await response.ThrowIfNotSuccessfulResponse();
        }
    }
}
