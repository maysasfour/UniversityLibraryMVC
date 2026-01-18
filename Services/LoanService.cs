using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public class LoanService : ILoanService
    {
        private readonly LibraryDbContext _context;

        public LoanService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Loan>> GetAllLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToListAsync();
        }

        public async Task<Loan> GetLoanByIdAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.LoanID == id);
        }

        public async Task<bool> AddLoanAsync(Loan loan)
        {
            try
            {
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateLoanAsync(Loan loan)
        {
            try
            {
                _context.Loans.Update(loan);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteLoanAsync(int id)
        {
            try
            {
                var loan = await _context.Loans.FindAsync(id);
                if (loan != null)
                {
                    _context.Loans.Remove(loan);
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

        public async Task<List<Loan>> GetActiveLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.Status == "Active")
                .ToListAsync();
        }

        public async Task<List<Loan>> GetOverdueLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.DueDate < DateTime.Today && l.Status == "Active")
                .ToListAsync();
        }
    }
}