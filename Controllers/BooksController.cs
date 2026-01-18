using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityLibraryMVC.Models;
using UniversityLibraryMVC.Services;
using System.Threading.Tasks;

namespace UniversityLibraryMVC.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly UniversityService _universityService;

        public BooksController(IBookService bookService, UniversityService universityService)
        {
            _bookService = bookService;
            _universityService = universityService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var books = string.IsNullOrEmpty(searchTerm)
                ? await _bookService.GetAllBooksAsync()
                : await _bookService.SearchBooksAsync(searchTerm);

            return View(books);
        }

        public IActionResult Create()
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            ViewBag.Categories = new List<string>
            {
                "Information Technology",
                "Mathematics",
                "Physics",
                "Engineering",
                "Business",
                "Philosophy",
                "Marketing",
                "Litreture"
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            if (ModelState.IsValid)
            {
                book.AvailableCopies = book.TotalCopies;
                book.UniversityID = university.UniversityID;

                var result = await _bookService.AddBookAsync(book);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving book to database.");
            }

            return View(book);
        }

        public async Task<IActionResult> Details(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            if (id != book.BookID) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _bookService.UpdateBookAsync(book);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating book.");
            }

            return View(book);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}