using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityLibraryMVC.Models;
using UniversityLibraryMVC.Services;
using System.Threading.Tasks;

namespace UniversityLibraryMVC.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;
        private readonly UniversityService _universityService;

        public LoansController(ILoanService loanService, IBookService bookService, IMemberService memberService, UniversityService universityService)
        {
            _loanService = loanService;
            _bookService = bookService;
            _memberService = memberService;
            _universityService = universityService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var loans = await _loanService.GetAllLoansAsync();
            return View(loans);
        }

        public async Task<IActionResult> Create(int? bookId)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var viewModel = new LoanViewModel
            {
                Books = await _bookService.GetAllBooksAsync(),
                Members = await _memberService.GetAllMembersAsync()
            };

            if (bookId.HasValue)
            {
                viewModel.SelectedBookId = bookId.Value;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoanViewModel model)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            if (ModelState.IsValid)
            {
                var loan = new Loan
                {
                    BookID = model.SelectedBookId,
                    MemberID = model.SelectedMemberId,
                    LoanDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    Status = "Active",
                    FineAmount = 0.0m
                };

                var book = await _bookService.GetBookByIdAsync(model.SelectedBookId);
                if (book != null && book.AvailableCopies > 0)
                {
                    book.AvailableCopies--;
                    await _bookService.UpdateBookAsync(book);

                    var result = await _loanService.AddLoanAsync(loan);
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "Selected book is not available for loan.");
            }

            model.Books = await _bookService.GetAllBooksAsync();
            model.Members = await _memberService.GetAllMembersAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null) return NotFound();

            return View(loan);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null) return NotFound();

            var viewModel = new LoanViewModel
            {
                SelectedLoanId = loan.LoanID,
                SelectedBookId = loan.BookID,
                SelectedMemberId = loan.MemberID,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Status = loan.Status,
                FineAmount = loan.FineAmount,
                Books = await _bookService.GetAllBooksAsync(),
                Members = await _memberService.GetAllMembersAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoanViewModel model)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            if (id != model.SelectedLoanId) return NotFound();

            if (ModelState.IsValid)
            {
                var loan = await _loanService.GetLoanByIdAsync(id);
                if (loan == null) return NotFound();

                loan.BookID = model.SelectedBookId;
                loan.MemberID = model.SelectedMemberId;
                loan.LoanDate = model.LoanDate;
                loan.DueDate = model.DueDate;
                loan.ReturnDate = model.ReturnDate;
                loan.Status = model.Status;
                loan.FineAmount = model.FineAmount;

                var result = await _loanService.UpdateLoanAsync(loan);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating loan.");
            }

            model.Books = await _bookService.GetAllBooksAsync();
            model.Members = await _memberService.GetAllMembersAsync();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null) return NotFound();

            return View(loan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _loanService.DeleteLoanAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Return(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null || loan.Status != "Active") return NotFound();

            loan.ReturnDate = DateTime.Now;
            loan.Status = "Returned";

            if (loan.DueDate < DateTime.Now)
            {
                var daysOverdue = (DateTime.Now - loan.DueDate).Days;
                loan.FineAmount = daysOverdue * 1.00m; 
            }

            var book = await _bookService.GetBookByIdAsync(loan.BookID);
            if (book != null)
            {
                book.AvailableCopies++;
                await _bookService.UpdateBookAsync(book);
            }

            await _loanService.UpdateLoanAsync(loan);
            return RedirectToAction(nameof(Index));
        }
    }
}