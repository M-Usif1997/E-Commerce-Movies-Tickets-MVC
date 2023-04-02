using E_Commerce_Movies.Data;
using E_Commerce_Movies.Data.Repositories;
using E_Commerce_Movies.Data.Services;
using E_Commerce_Movies.Data.Static;
using E_Commerce_Movies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ActorsController : Controller
    {
        private readonly IActorsRepo _actorsRepo;

        public ActorsController(IActorsRepo actorsRepo)
        {
            _actorsRepo = actorsRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _actorsRepo.GetAllAsync();
            return View(data);
        }

        //Get: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")]Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _actorsRepo.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }

        //Get: Actors/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await _actorsRepo.GetByIdAsync(id);

            if (actorDetails == null) return View("NotFound",id);
            return View(actorDetails);
        }

        //Get: Actors/Edit/1
        public async Task<IActionResult> Edit(int id)
        {

            var actorDetails = await _actorsRepo.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound",id);
            return View(actorDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _actorsRepo.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));
        }

        //Get: Actors/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await _actorsRepo.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound",id);
            return View(actorDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _actorsRepo.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound",id);

            await _actorsRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
