namespace ResumeGenerator.Data.Models
{
    public enum StyleNames
    {
        Chronological,
        Functional,
        Combination,
        Classic
    };
    public class InputDataForResume
    {
        public StyleNames styleNumber { get; set; }

        
    }
}
