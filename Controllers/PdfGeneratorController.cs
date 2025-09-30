using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResumeGenerator.Data;
using ResumeGenerator.Data.Interfaces;
using ResumeGenerator.Data.Models;
using ResumeGenerator.Data.Models.Exceptions;
using ResumeGenerator.Data.ResumeStyles;


namespace ResumeGenerator.Controllers
{
    [Route("generate")]
    [ApiController]
    public class PdfGeneratorController : ControllerBase
    {

        private readonly IEnumerable<IPdfGenerator> _pdfGenerators;
        private readonly IOpenAiIntegration _openAiIntegration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PdfGeneratorController(IEnumerable<IPdfGenerator> pdfGenerators, IOpenAiIntegration openAiIntegration, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _pdfGenerators = pdfGenerators;
            _openAiIntegration = openAiIntegration;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async public Task<IActionResult> OnGenerate([FromBody] CompleteData completeData, int designNumber)
        {
            if (!ModelState.IsValid) throw new ModelStateException("Please fill everything correctly");
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if(email == null)
            {
                throw new NotFoundException("Please try to relogin");
            }


            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (user.UsageAmount <= 0)
            {
                throw new PaymentException("Please top up the balance");
            }

            var generator = _pdfGenerators.FirstOrDefault(g => g.designNumber == designNumber);

            if (generator == null) {
                throw new NotFoundException("Invalid Design number");
            }


            var PdfDocument = generator.StyleGenerator_1(completeData);

            using (var stream = new MemoryStream())
            {
                PdfDocument.Save(stream, false); // false = don't close stream
                var fileBytes = stream.ToArray();

                // Return as File
                user.UsageAmount--;
                await _userManager.UpdateAsync(user); 
                await _context.SaveChangesAsync();
                return File(fileBytes, "application/pdf", "sample.pdf");
            }

            

           
        }

        [HttpPost("generatewithai")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async public Task<IActionResult> OnGenerateWithAi([FromBody] KeyWordsForAi notCompletedData, int designNumber)
        {
            if (!ModelState.IsValid) throw new ModelStateException("Please fill everything correctly");
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
            {
                throw new NotFoundException("Please try to relogin");
            }



            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (user.UsageAmount <= 0)
            {
                throw new PaymentException("Please top up the balance");
            }

            CompleteData completeData = new CompleteData();

            var generator = _pdfGenerators.FirstOrDefault(g => g.designNumber == designNumber);

            if (generator == null)
            {
                throw new NotFoundException("Invalid Design number");
            }

            completeData = await _openAiIntegration.AiDataGetter(notCompletedData);




            


            var PdfDocument = generator.StyleGenerator_1(completeData);

            using (var stream = new MemoryStream())
            {
                PdfDocument.Save(stream, false); // false = don't close stream
                var fileBytes = stream.ToArray();

                // Return as File
                user.UsageAmount--;
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
                return File(fileBytes, "application/pdf", "sample.pdf");
            }




        }

    }
}
