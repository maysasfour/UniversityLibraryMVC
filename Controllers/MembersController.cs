using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityLibraryMVC.Models;
using UniversityLibraryMVC.Services;
using System.Threading.Tasks;

namespace UniversityLibraryMVC.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly UniversityService _universityService;

        public MembersController(IMemberService memberService, UniversityService universityService)
        {
            _memberService = memberService;
            _universityService = universityService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var members = string.IsNullOrEmpty(searchTerm)
                ? await _memberService.GetAllMembersAsync()
                : await _memberService.SearchMembersAsync(searchTerm);

            return View(members);
        }

        public IActionResult Create()
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            if (ModelState.IsValid)
            {
                member.UniversityID = university.UniversityID;
                member.RegistrationDate = DateTime.Now;
                member.MembershipDate = DateTime.Now;

                var result = await _memberService.AddMemberAsync(member);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving member to database.");
            }

            return View(member);
        }

        public async Task<IActionResult> Details(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            if (id != member.MemberID) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberAsync(member);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating member.");
            }

            return View(member);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var university = _universityService.GetMenuUniversity();
            ViewBag.University = university;

            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}