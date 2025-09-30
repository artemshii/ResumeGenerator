using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using ResumeGenerator.Data;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using Microsoft.AspNetCore.Identity;
using ResumeGenerator.Data.Models;
using ResumeGenerator.Data.Models.Exceptions;
using ResumeGenerator.Data.Interfaces;
using PdfSharp.Pdf;
namespace ResumeGenerator.Controllers

{
    [Route("pay")]
    [ApiController]
    public class MainController : ControllerBase
    {
   
        private readonly IOpenAiIntegration _openAiIntegration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IInvoiceGenerator _invoiceGenerator;

        public MainController(IOpenAiIntegration openAiIntegration, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration config, IInvoiceGenerator invoiceGenerator)
        {
            
            _openAiIntegration = openAiIntegration;
            _userManager = userManager;
            _context = context;
            _config = config;
            _invoiceGenerator = invoiceGenerator;
        }

        

        [HttpPost("order/create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create()
        {
            var domain = _config["Domain"];
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, price_1234) of the product you want to sell
                    Price = "pPiceId",
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                CustomerEmail = User.FindFirst(ClaimTypes.Email)?.Value,
                SuccessUrl = domain + $"/api/main/order/success?session_id={{CHECKOUT_SESSION_ID}}"
,
                CancelUrl = domain + "/api/main/order/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Append("Location", session.Url);
            return Ok(session.Url);
        }

        [HttpGet("order/cancel")]
        public IActionResult OrderFail()
        {
            return Ok("Please try again");
        }

        
        [HttpGet("order/success")]
        
        public async Task<IActionResult> OrderSuccess([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(session_id);
            

            if (session.PaymentStatus == "paid" && !string.IsNullOrEmpty(session.CustomerEmail))
            {
                var user = await _userManager.FindByEmailAsync(session.CustomerEmail);

                if (user == null)
                {
                    throw new NotFoundException("User not found");
                }

                // Check if this session was already processed
                bool alreadyProcessed = user.PaymentIds.Any(p => p.StripeSessionId == session.Id);

                if (!alreadyProcessed)
                {
                    // Add new payment record
                    user.PaymentIds.Add(new PaymentId
                    {
                        StripeSessionId = session.Id,
                        ApplicationUserId = user.Id
                    });

                    user.UsageAmount += 10;

                    await _userManager.UpdateAsync(user); // persist changes
                    await _context.SaveChangesAsync();

                    long amountTotal = session.AmountTotal ?? 0; 
                    decimal amountInEur = amountTotal / 100m;   

                    var data = await _invoiceGenerator.Generate(amountInEur.ToString(), session_id, session.CustomerEmail);

                    

                    using (var stream = new MemoryStream())
                    {
                        data.Save(stream, false); // false = don't close stream
                        var fileBytes = stream.ToArray();



                        return File(fileBytes, "application/pdf", "sample.pdf");
                    }



                    
                    
                }
                else
                {
                    throw new SessionProccesedException("Session was already procceeded");
                }
            }

            throw new PaymentException("Please pay or create a new session");
        }

        [HttpGet("debug")]
        

        async public Task<IActionResult> ChangeData()
        {
            var data = await _invoiceGenerator.Generate("123", "wefw1", "bebra@gmail.com");

            using (var stream = new MemoryStream())
            {
                data.Save(stream, false); // false = don't close stream
                var fileBytes = stream.ToArray();

                
                
                return File(fileBytes, "application/pdf", "sample.pdf");
            }

            
        }

    }
}
