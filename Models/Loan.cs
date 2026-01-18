using System.ComponentModel.DataAnnotations;

namespace UniversityLibraryMVC.Models
{
    public class Loan
    {
        public int LoanID { get; set; }

        public int BookID { get; set; }
        public int MemberID { get; set; }

        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public decimal FineAmount { get; set; } = 0.0m;

        public virtual Book? Book { get; set; }
        public virtual Member? Member { get; set; }
    }
}