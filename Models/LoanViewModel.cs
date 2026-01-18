using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UniversityLibraryMVC.Models
{
    public class LoanViewModel
    {
        public int SelectedLoanId { get; set; }
        public int SelectedBookId { get; set; }
        public int SelectedMemberId { get; set; }

        [Required]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);

        public DateTime? ReturnDate { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        public decimal FineAmount { get; set; } = 0.0m;

        public List<Book> Books { get; set; } = new List<Book>();
        public List<Member> Members { get; set; } = new List<Member>();
    }
}