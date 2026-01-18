using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityLibraryMVC.Models;
using UniversityLibraryMVC.Services;
using System.Threading.Tasks;

namespace UniversityLibraryMVC.Controllers
{
    [Authorize]
    public class UniversitiesController : Controller
    {
        private readonly UniversityService _universityService;

        public UniversitiesController(UniversityService universityService)
        {
            _universityService = universityService;
        }

        public async Task<IActionResult> Index()
        {
            var universities = await _universityService.GetAllUniversitiesAsync();
            return View(universities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(University university)
        {
            if (ModelState.IsValid)
            {
                var result = await _universityService.AddUniversityAsync(university);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving university to database.");
            }

            return View(university);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var university = await _universityService.GetUniversityByIdAsync(id);
            if (university == null) return NotFound();

            return View(university);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, University university)
        {
            if (id != university.UniversityID) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _universityService.UpdateUniversityAsync(university);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating university.");
            }

            return View(university);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var university = await _universityService.GetUniversityByIdAsync(id);
            if (university == null) return NotFound();

            return View(university);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _universityService.DeleteUniversityAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}