using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMembersAsync();
        Task<Member> GetMemberByIdAsync(int id);
        Task<bool> AddMemberAsync(Member member);
        Task<bool> UpdateMemberAsync(Member member);
        Task<bool> DeleteMemberAsync(int id);
        Task<List<Member>> SearchMembersAsync(string searchTerm);
    }
}