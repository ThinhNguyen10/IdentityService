using Microsoft.EntityFrameworkCore;

namespace Identity.Models
{
    public class AuthenticationContext : DbContext
    {

        public AuthenticationContext(DbContextOptions option) : base(option)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(t =>
            {
                t.ToTable("ACCOUNT");

                t.HasKey(x => x.Id);

                t.HasAlternateKey(x => x.UserName);

                t.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

                t.Property(x => x.FullName)
                .HasColumnName("FULLNAME")
                .HasMaxLength(30)
                .IsUnicode(true);

                t.Property(x => x.UserName)
               .HasColumnName("USERNAME")
               .HasMaxLength(30);

                t.Property(x => x.PassWord)
               .HasColumnName("PASSWORD")
               .HasMaxLength(30);

                t.Property(x => x.Email)
               .HasColumnName("EMAIL")
               .HasMaxLength(30);

                t.Property(x => x.Phone)
               .HasColumnName("PHONE")
               .HasMaxLength(11);

            });

            
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
