namespace ResumeGenerator.Data.Models.Exceptions
{
    public class PdfException : Exception
    {
        public PdfException(string message): base(message) { }
    }
}
