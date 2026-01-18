using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public class MemberService : IMemberService
    {
        private readonly LibraryDbContext _context;

        public MemberService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }

        public async Task<Member> GetMemberByIdAsync(int id)
        {
            return await _context.Members.FindAsync(id);
        }

        public async Task<bool> AddMemberAsync(Member member)
        {
            try
            {
                _context.Members.Add(member);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateMemberAsync(Member member)
        {
            try
            {
                _context.Members.Update(member);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMemberAsync(int id)
        {
            try
            {
                var member = await _context.Members.FindAsync(id);
                if (member != null)
                {
                    _context.Members.Remove(member);
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

        public async Task<List<Member>> SearchMembersAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await _context.Members.ToListAsync();

            return await _context.Members
                .Where(m => m.FirstName.Contains(searchTerm) ||
                           m.LastName.Contains(searchTerm) ||
                           m.Email.Contains(searchTerm))
                .ToListAsync();
        }
    }
}