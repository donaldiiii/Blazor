﻿using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task DeletePeople(int id);
        Task<List<Person>> GetPeopleByName(string name);
        Task<List<Person>> GetPerson();
        Task<Person> GetPersonById(int id);
        Task UpdatePerson(Person person);
    }
}