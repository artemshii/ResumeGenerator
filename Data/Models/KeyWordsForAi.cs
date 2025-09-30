using System.ComponentModel.DataAnnotations;

namespace ResumeGenerator.Data.Models
{
    public class KeyWordsForAi
    {
        
        public List<EducationInput>? Education { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? JobTitle { get; set; }
        public List<WorkExprerienceInput>? WorkExperience { get; set; }
        [Required]
        public List<string>? Skills { get; set; } 
        [Required]
        
        public List<string>? Languages { get; set; }

    }

    public class EducationInput
    {
        [Required]
        public string? Institution { get; set; }
        [Required]
        public string? Degree { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public string? YearStart { get; set; }
        public string? YearEnd { get; set; }
    }

    public class WorkExprerienceInput
    {
        [Required]
        public string? CompanyName { get; set; }
        [Required]
        public string? Position { get;set; }
        [Required]
        public string? YearStart { get;set;}
        public string? YearEnd { get;set;}
    }
}
