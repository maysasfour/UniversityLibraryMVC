using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public interface ILoanService
    {
        Task<List<Loan>> GetAllLoansAsync();
        Task<Loan> GetLoanByIdAsync(int id);
        Task<bool> AddLoanAsync(Loan loan);
        Task<bool> UpdateLoanAsync(Loan loan);
        Task<bool> DeleteLoanAsync(int id);
        Task<List<Loan>> GetActiveLoansAsync();
        Task<List<Loan>> GetOverdueLoansAsync();
    }
}