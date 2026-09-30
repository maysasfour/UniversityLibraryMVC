using Microsoft.AspNetCore.Mvc;
using UniversityLibraryMVC.Models;
using UniversityLibraryMVC.Services;
using System.Threading.Tasks;
using System.Linq;

namespace UniversityLibraryMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;
        private readonly ILoanService _loanService;
        private readonly UniversityService _universityService;

        public HomeController(IBookService bookService, IMemberService memberService, ILoanService loanService, UniversityService universityService)
        {
            _bookService = bookService;
            _memberService = memberService;
            _loanService = loanService;
            _universityService = universityService;
        }

        public async Task<IActionResult> Index()
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var viewModel = new HomeViewModel
            {
                TotalBooks = (await _bookService.GetAllBooksAsync()).Count,
                TotalMembers = (await _memberService.GetAllMembersAsync()).Count,
                ActiveLoans = (await _loanService.GetActiveLoansAsync()).Count,
                OverdueLoans = (await _loanService.GetOverdueLoansAsync()).Count,
                RecentBooks = (await _bookService.GetAllBooksAsync()).Take(5).ToList(),
                RecentLoans = (await _loanService.GetAllLoansAsync()).Take(5).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            ViewBag.University = _universityService.GetMenuUniversity();
            return View();
        }
    }
}
