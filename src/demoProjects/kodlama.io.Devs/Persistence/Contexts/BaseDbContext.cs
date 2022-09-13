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

            modelBuilder.Entity<Technology>(a =>
            {
                a.ToTable("Technologies").HasKey(k => k.Id);
                a.Property(t => t.Id).HasColumnName("Id");
                a.Property(t => t.Name).HasColumnName("Name");
                a.Property(t => t.ProgrammingLanguageId).HasColumnName("ProgrammingLanguageId");

                a.HasOne(a => a.ProgrammingLanguage);
            });

            ProgrammingLanguage[] programmingLanguages = { new(1, "Java"), new(2, "C#") };
            modelBuilder.Entity<ProgrammingLanguage>().HasData(programmingLanguages);

            Technology[] technologies = { new(1, "Spring", 1), new(2, "JSP", 1), new(3, "WPF", 2), new(4, "ASP.NET", 2) };
            modelBuilder.Entity<Technology>().HasData(technologies);

            
        }




    }
}
