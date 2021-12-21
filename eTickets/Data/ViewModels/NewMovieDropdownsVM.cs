using E_Commerce_Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Data.ViewModels
{
    public class NewMovieDropdownsVM
    {
        public NewMovieDropdownsVM()
        {
            Producers = new List<Producer>();
            Actors = new List<Actor>();
            Cinemas = new List<Cinema>();         
        }

        public List<Producer> Producers { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Cinema> Cinemas { get; set; }
      
    }
}
