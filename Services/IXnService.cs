using System.Threading.Tasks;
using System.Collections.Generic;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public interface IXnService
    {
        Task<List<Book>> GetBooksAsync();
        Task<List<Member>> GetMembersAsync();
        Task<List<Loan>> GetLoansAsync();
    }
}