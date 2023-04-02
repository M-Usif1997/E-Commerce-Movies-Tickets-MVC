using E_Commerce_Movies.Data.Base;
using E_Commerce_Movies.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Data.Repositories

{
    public class ActorsRepo : EntityBaseRepository<Actor>, IActorsRepo
    {
        public ActorsRepo(AppDbContext context) : base(context) { }
    }
}
