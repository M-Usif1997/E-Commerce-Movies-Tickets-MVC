using E_Commerce_Movies.Data.Base;
using E_Commerce_Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Data.Services
{
    public class ProducersService : EntityBaseRepository<Producer>, IProducersService
    {

        public ProducersService(AppDbContext context) : base(context)
        {
        }
    }
}
