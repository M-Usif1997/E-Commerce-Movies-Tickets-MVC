using E_Commerce_Movies.Data;
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
    public class ProducersController : Controller
    {
        private readonly IProducersRepo _producersRepo;

        public ProducersController(IProducersRepo producersRepo)
        {
            _producersRepo = producersRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allProducers = await _producersRepo.GetAllAsync();
            return View(allProducers);
        }


        //Get: Producers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")] Producer producer)
        {
            if (!ModelState.IsValid)
            {
                return View(producer);
            }
            await _producersRepo.AddAsync(producer);
            return RedirectToAction(nameof(Index));
        }



        //Get: Proucers/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var ProducerDetails = await _producersRepo.GetByIdAsync(id);

            if (ProducerDetails == null) return View("NotFound", id);
            return View(ProducerDetails);
        }



        //GET: producers/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var producerDetails = await _producersRepo.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound",id);
            return View(producerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,Producer producer)
        {
            if (!ModelState.IsValid) return View(producer);

            if (id == producer.Id)
            {
                await _producersRepo.UpdateAsync(id, producer);
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }


        //GET: producers/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var producerDetails = await _producersRepo.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound",id);
            return View(producerDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producerDetails = await _producersRepo.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound",id);

            await _producersRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
