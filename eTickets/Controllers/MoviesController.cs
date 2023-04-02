using E_Commerce_Movies.Data.Services;
using E_Commerce_Movies.Data.Static;
using E_Commerce_Movies.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MoviesController : Controller
    {
        private readonly IMoviesRepo _moviesRepo;

       
        public MoviesController(IMoviesRepo moviesRepo)
        {
            _moviesRepo = moviesRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allMovies = await _moviesRepo.GetAllAsync(m => m.Cinema);      //To Get Cinemas with Movies ,notes=>use eager loading 
            return View(allMovies);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _moviesRepo.GetAllAsync(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allMovies.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allMovies);
        }


        //GET: Movies/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetail = await _moviesRepo.GetMovieByIdAsync(id);
            if (movieDetail == null) return View("NotFound",id);
            return View(movieDetail);
        }



        //GET: Movies/Create
        public async Task<IActionResult> Create()
        {
            var movieDropdownsData = await _moviesRepo.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {

            if(!ModelState.IsValid)
            {
                //To Show dropdown menus when created post request and validation happen by false 
                var movieDropdownsData = await _moviesRepo.GetNewMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

                return View(movie);
            }

            await _moviesRepo.AddNewMovieAsync(movie);

            return RedirectToAction(nameof(Index));

        }



        //GET: Movies/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var movieDetail = await _moviesRepo.GetMovieByIdAsync(id);   //it return Movie domain class bind to database not NewMovieVM view model
            if (movieDetail == null) return View("NotFound", id);

            var response = new NewMovieVM()              //Take data of movieDetail to display from database
            {
                Id = movieDetail.Id,
                Name = movieDetail.Name,
                Description = movieDetail.Description,
                Price = movieDetail.Price,
                StartDate = movieDetail.StartDate,
                EndDate = movieDetail.EndDate,
                ImageURL = movieDetail.ImageURL,
                MovieCategory = movieDetail.MovieCategory,
                CinemaId = movieDetail.CinemaId,
                ProducerId = movieDetail.ProducerId,
                ActorIds = movieDetail.Actors_Movies.Select(n => n.ActorId).ToList(),
            };


            var movieDropdownsData = await _moviesRepo.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View(response);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(int id,NewMovieVM movie)
        {

            if (id != movie.Id) return View("NotFound", id);

            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _moviesRepo.GetNewMovieDropdownsValues();    //When make post request and javascripts not allowed so to showed data again 

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

                return View(movie);
            }

            await _moviesRepo.UpdateMovieAsync(movie);

            return RedirectToAction(nameof(Index));

        }

    }
}
