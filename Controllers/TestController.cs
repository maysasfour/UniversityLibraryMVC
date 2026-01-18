using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Controllers
{
    public class TestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TestController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin()
        {
            var adminEmail = "admin@meu.edu";
            var adminPassword = "Admin123!";

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    ViewBag.Message = "Admin user created successfully!";
                }
                else
                {
                    ViewBag.Message = "Error creating admin user: " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            else
            {
              
                var result = await _userManager.RemovePasswordAsync(adminUser);
                if (result.Succeeded)
                {
                    result = await _userManager.AddPasswordAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        ViewBag.Message = "Password reset successfully!";
                    }
                    else
                    {
                        ViewBag.Message = "Error resetting password: " + string.Join(", ", result.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    ViewBag.Message = "Error removing password: " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }

            return View("Index");
        }
    }
}