using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public interface IUniversityService
    {
        Task<List<University>> GetAllUniversitiesAsync();
        Task<University> GetUniversityByIdAsync(int id);
        Task<bool> AddUniversityAsync(University university);
        Task<bool> UpdateUniversityAsync(University university);
        Task<bool> DeleteUniversityAsync(int id);
        University GetMenuUniversity();
    }
}