

using Microsoft.AspNetCore.Identity;
using ResumeGenerator.Data.Models;

public class ApplicationUser : IdentityUser
{
    public int UsageAmount { get; set; }
    public ICollection<PaymentId> PaymentIds { get; set; } = new List<PaymentId>();
}

