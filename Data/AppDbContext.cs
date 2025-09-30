using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.Data.Models;



public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PaymentId> PaymentIds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PaymentId>()
            .HasOne(p => p.User)
            .WithMany(u => u.PaymentIds)
            .HasForeignKey(p => p.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure StripeSessionId is unique
        modelBuilder.Entity<PaymentId>()
            .HasIndex(p => p.StripeSessionId)
            .IsUnique();

    }
}