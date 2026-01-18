using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityLibraryMVC.Services;

namespace UniversityLibraryMVC.Controllers
{
    [Authorize]
    public class XmlController : Controller
    {
        private readonly XmlService _xmlService;
        private readonly UniversityService _universityService;

        public XmlController(XmlService xmlService, UniversityService universityService)
        {
            _xmlService = xmlService;
            _universityService = universityService;
        }

        public IActionResult Index()
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;
            return View();
        }

        public async Task<IActionResult> ExportBooks()
        {
            var xmlContent = await _xmlService.ExportBooksToXmlAsync();
            return File(System.Text.Encoding.UTF8.GetBytes(xmlContent), "application/xml", "books.xml");
        }

        public async Task<IActionResult> ExportMembers()
        {
            var xmlContent = await _xmlService.ExportMembersToXmlAsync();
            return File(System.Text.Encoding.UTF8.GetBytes(xmlContent), "application/xml", "members.xml");
        }

        public async Task<IActionResult> ExportLoans()
        {
            var xmlContent = await _xmlService.ExportLoansToXmlAsync();
            return File(System.Text.Encoding.UTF8.GetBytes(xmlContent), "application/xml", "loans.xml");
        }

        [HttpPost]
        public async Task<IActionResult> ImportBooks(IFormFile xmlFile)
        {
            if (xmlFile != null && xmlFile.Length > 0)
            {
                using (var reader = new StreamReader(xmlFile.OpenReadStream()))
                {
                    var xmlContent = await reader.ReadToEndAsync();
                    await _xmlService.ImportBooksFromXmlAsync(xmlContent);
                }
                TempData["Message"] = "Books imported successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ImportMembers(IFormFile xmlFile)
        {
            if (xmlFile != null && xmlFile.Length > 0)
            {
                using (var reader = new StreamReader(xmlFile.OpenReadStream()))
                {
                    var xmlContent = await reader.ReadToEndAsync();
                    await _xmlService.ImportMembersFromXmlAsync(xmlContent);
                }
                TempData["Message"] = "Members imported successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ImportLoans(IFormFile xmlFile)
        {
            if (xmlFile != null && xmlFile.Length > 0)
            {
                using (var reader = new StreamReader(xmlFile.OpenReadStream()))
                {
                    var xmlContent = await reader.ReadToEndAsync();
                   
                    TempData["Message"] = "Loans import functionality coming soon!";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}