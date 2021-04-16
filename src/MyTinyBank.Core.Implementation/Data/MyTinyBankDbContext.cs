using Microsoft.EntityFrameworkCore;

using MyTinyBank.Core.Model;

namespace MyTinyBank.Core.Implementation.Data
{
    public class MyTinyBankDbContext : DbContext
    {
        public MyTinyBankDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(builder => 
            {
                builder.ToTable("Customer");
                builder.HasIndex(c => c.VatNumber).IsUnique();
                builder.OwnsOne(c => c.AuditInfo);
            });

            modelBuilder.Entity<Account>(builder => 
            {
                builder.ToTable("Account");
                builder.OwnsOne(c => c.AuditInfo);
            });

            modelBuilder.Entity<Card>(builder => 
            {
                builder.ToTable("Card");
                builder.HasIndex(c => c.CardNumber).IsUnique();
            });
        }
    }
}
