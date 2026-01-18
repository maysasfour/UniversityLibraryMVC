using Microsoft.EntityFrameworkCore;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public class XnService : IXnService
    {
        private readonly LibraryDbContext _context;

        public XnService(LibraryDbContext context)
        {
            _context = context;
        }

     
        public async Task<List<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<List<Member>> GetMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }

        public async Task<List<Loan>> GetLoansAsync()
        {
            return await _context.Loans.ToListAsync();
        }
    }
}