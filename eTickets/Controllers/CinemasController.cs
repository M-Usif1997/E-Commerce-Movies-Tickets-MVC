using E_Commerce_Movies.Data;
using E_Commerce_Movies.Data.Repositories;
using E_Commerce_Movies.Data.Services;
using E_Commerce_Movies.Data.Static;
using E_Commerce_Movies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CinemasController : Controller
    {
        private readonly ICinemasRepo _cinemasRepo;

        public CinemasController(ICinemasRepo cinemasRepo)
        {
           _cinemasRepo = cinemasRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allCinemas = await _cinemasRepo.GetAllAsync();
            return View(allCinemas);
        }


        //Get: Cinemas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid) return View(cinema);
            await _cinemasRepo.AddAsync(cinema);
            return RedirectToAction(nameof(Index));
        }


        //Get: Cinemas/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var cinemaDetails = await _cinemasRepo.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound",id);
            return View(cinemaDetails);
        }

        //Get: Cinemas/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var cinemaDetails = await _cinemasRepo.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound",id);
            return View(cinemaDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,Cinema cinema)
        {
            if (!ModelState.IsValid) return View(cinema);
            await _cinemasRepo.UpdateAsync(id, cinema);
            return RedirectToAction(nameof(Index));
        }


        //Get: Cinemas/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var cinemaDetails = await _cinemasRepo.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var cinemaDetails = await _cinemasRepo.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");

            await _cinemasRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
