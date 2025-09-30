using System.ComponentModel.DataAnnotations;

namespace ResumeGenerator.Data.Models
{
    public class CompleteData
    {
        public string? ProfileSummary {  get; set; }

        [Required]
        public string? JobTitle { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? LinkedIn { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }


        public List<ProfessionalExperience>? ProffesionalExperience { get; set; }


        public List<Education>? Education { get; set; }


        public List<string>? Skills { get; set; }
        public List<string>? Languages {  get; set; }


    }

    public class Education
    {
        [Required]
        public string? InstitutionName { get; set; }

        [Required]
        public string? Degree { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? YearStart { get; set; }
        public string? YearEnd { get; set; }
        

        public string? Description { get; set; }
    }

    public class ProfessionalExperience
    {
        [Required]
        public string? CompanyName { get; set; }

        [Required]
        public string? Position { get; set; }

        [Required]
        public string? Location {  get; set; }

        [Required]
        public string? YearStart { get; set ; }
        public string? YearEnd { get; set ; }
        public string? CompanyDescription { get; set; }
    }


}
