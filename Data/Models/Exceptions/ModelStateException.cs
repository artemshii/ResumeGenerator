namespace ResumeGenerator.Data.Models.Exceptions
{
    public class ModelStateException : Exception
    {
        public ModelStateException(string message) : base(message) { }
    }
}
