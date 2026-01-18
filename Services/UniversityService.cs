using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly LibraryDbContext _context;

        public UniversityService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<University>> GetAllUniversitiesAsync()
        {
            return await _context.Universities.ToListAsync();
        }

        public async Task<University> GetUniversityByIdAsync(int id)
        {
            return await _context.Universities.FindAsync(id);
        }

        public async Task<bool> AddUniversityAsync(University university)
        {
            try
            {
                _context.Universities.Add(university);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUniversityAsync(University university)
        {
            try
            {
                _context.Universities.Update(university);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUniversityAsync(int id)
        {
            try
            {
                var university = await _context.Universities.FindAsync(id);
                if (university != null)
                {
                    _context.Universities.Remove(university);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public University GetMenuUniversity()
        {
            return _context.Universities.FirstOrDefault(u => u.MenuPosition == 1) ?? new University
            {
                Name = "Meu University",
                PrimaryColor = "#003366",
                SecondaryColor = "#FFD700",
                LogoPath = "/images/meu-university-logo.png"
            };
        }
    }
}