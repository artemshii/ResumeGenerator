using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf;
using ResumeGenerator.Data.Models;

namespace ResumeGenerator.Data.Interfaces
{
    public interface IPdfGenerator
    {
        public int designNumber { get; }
        PdfDocument StyleGenerator_1(CompleteData completeData);
        
        
    }
}
