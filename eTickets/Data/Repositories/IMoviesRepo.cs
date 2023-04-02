using E_Commerce_Movies.Data.Base;
using E_Commerce_Movies.Data.ViewModels;
using E_Commerce_Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Data.Services
{
  public  interface IMoviesRepo : IEntityBaseRepository<Movie>
    {

        Task AddNewMovieAsync(NewMovieVM data);
        Task UpdateMovieAsync(NewMovieVM data);
        Task<Movie> GetMovieByIdAsync(int id);
        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
    }
}
