using PdfSharp.Pdf;

namespace ResumeGenerator.Data.Interfaces
{
    public interface IInvoiceGenerator
    {

        public Task<PdfDocument> Generate(string price, string sessionId, string email);
    }
}
