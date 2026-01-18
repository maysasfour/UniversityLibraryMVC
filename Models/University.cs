using System.ComponentModel.DataAnnotations;

namespace UniversityLibraryMVC.Models
{
    public class University
    {
        public int UniversityID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(7)]
        public string PrimaryColor { get; set; } = "#003366";

        [StringLength(7)]
        public string SecondaryColor { get; set; } = "#FFD700";

        [StringLength(255)]
        public string LogoPath { get; set; } = "/images/meu-university-logo.png";

        public int MenuPosition { get; set; } = 1;
    }
}