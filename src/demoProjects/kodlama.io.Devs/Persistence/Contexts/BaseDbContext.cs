using Core.Security.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<SocialMedia> SocialMedias{ get; set; }
        public DbSet<RefreshToken> RefreshTokens{ get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims{ get; set; }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //    base.OnConfiguring(
            //        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SomeConnectionString")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgrammingLanguage>(a =>
            {
                a.ToTable("ProgrammingLanguages").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.Name).HasColumnName("Name");

                a.HasMany(p => p.Technologies);

            });

            ProgrammingLanguage[] programmingLanguages = { new(1, "Java"), new(2, "C#") };
            modelBuilder.Entity<ProgrammingLanguage>().HasData(programmingLanguages);

            modelBuilder.Entity<Technology>(a =>
            {
                a.ToTable("Technologies").HasKey(k => k.Id);
                a.Property(t => t.Id).HasColumnName("Id");
                a.Property(t => t.Name).HasColumnName("Name");
                a.Property(t => t.ProgrammingLanguageId).HasColumnName("ProgrammingLanguageId");

                a.HasOne(a => a.ProgrammingLanguage);
            });

            Technology[] technologies = { new(1, "Spring", 1), new(2, "JSP", 1), new(3, "WPF", 2), new(4, "ASP.NET", 2) };
            modelBuilder.Entity<Technology>().HasData(technologies);

            modelBuilder.Entity<OperationClaim>(a =>
            {
                a.ToTable("OperationClaims").HasKey(k => k.Id);
                a.Property(t => t.Id).HasColumnName("Id");
                a.Property(t => t.Name).HasColumnName("Name");
            });

            OperationClaim[] operationClaims = { new(1, "Moderator"), new(2, "Admin") };
            modelBuilder.Entity<OperationClaim>().HasData(operationClaims);


            modelBuilder.Entity<UserOperationClaim>(a =>
            {
                a.ToTable("UserOperationClaims").HasKey(k => k.Id);
                a.Property(t => t.Id).HasColumnName("Id");
                a.Property(t => t.UserId).HasColumnName("UserId");
                a.Property(t => t.OperationClaimId).HasColumnName("OperationClaimId");

                a.HasOne(a => a.OperationClaim);
                a.HasOne(a => a.User);

            });

            modelBuilder.Entity<User>(a =>
            {
                a.ToTable("Users").HasKey(k => k.Id);
                a.Property(t => t.Id).HasColumnName("Id");
                a.Property(t => t.FirstName).HasColumnName("FirstName");
                a.Property(t => t.LastName).HasColumnName("LastName");
                a.Property(t => t.Email).HasColumnName("Email");
                a.Property(t => t.Status).HasColumnName("Status");
                a.Property(t => t.PasswordHash).HasColumnName("PasswordHash");
                a.Property(t => t.PasswordSalt).HasColumnName("PasswordSalt");
                a.Property(t => t.AuthenticatorType).HasColumnName("AuthenticatorType");

                a.HasMany(a => a.RefreshTokens);
                a.HasMany(a => a.UserOperationClaims);
            });

            modelBuilder.Entity<Developer>(a =>
            {
                a.HasMany(a => a.SocialMedias);
            });

            modelBuilder.Entity<SocialMedia>(a =>
            {
                a.ToTable("SocialMedias").HasKey(k => k.Id);
                a.Property(s => s.Id).HasColumnName("Id");
                a.Property(s => s.DeveloperId).HasColumnName("DeveloperId");
                a.Property(s => s.Name).HasColumnName("Name");
                a.Property(s => s.Url).HasColumnName("Url");

                a.HasOne(s => s.Developer);
            });

            

            

            
        }




    }
}
