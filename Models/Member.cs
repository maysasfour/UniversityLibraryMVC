using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UniversityLibraryMVC.Models
{
    public class Member
    {
        public int MemberID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public string Name => $"{FirstName} {LastName}";

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(100)]
        public string Address { get; set; } = string.Empty;

        [Range(18, 100)]
        public int? Age { get; set; }

        [StringLength(50)]
        public string MembershipType { get; set; } = "Student";

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime MembershipDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string StudentID { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int UniversityID { get; set; } = 1;

        public virtual University? University { get; set; }
        public virtual ICollection<Loan> Loans { get; set; } = new HashSet<Loan>();
    }
}