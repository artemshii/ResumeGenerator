using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResumeGenerator.Data.Models
{
    public class PaymentId
    {
        [Key]
        public int paymentId { get; set; }

        [Required]
        public string? StripeSessionId { get; set; }   // PK



        // Foreign key to ApplicationUser
        [Required]
        public string? ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? User { get; set; }
    }
}
