using System.Collections.Generic;

namespace UniversityLibraryMVC.Models
{
    public class HomeViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveLoans { get; set; }
        public int OverdueLoans { get; set; }
        public List<Book> RecentBooks { get; set; } = new List<Book>();
        public List<Loan> RecentLoans { get; set; } = new List<Loan>();
    }
}