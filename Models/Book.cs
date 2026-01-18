using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UniversityLibraryMVC.Models
{
    public class Book
    {
        public int BookID { get; set; }

        [Required]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [StringLength(100)]
        public string Publisher { get; set; } = string.Empty;

        [Range(1800, 2100)]
        public int? PublicationYear { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int TotalCopies { get; set; } = 1;

        [Range(0, 1000)]
        public int AvailableCopies { get; set; } = 1;

        public int UniversityID { get; set; } = 1;

        public virtual University? University { get; set; }
        public virtual ICollection<Loan> Loans { get; set; } = new HashSet<Loan>();
    }
}